using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Application.Services;

public class ServiceOrderService : IServiceOrderService
{
    private readonly IServiceOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IPartRepository _partRepository;

    public ServiceOrderService(
        IServiceOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IEquipmentRepository equipmentRepository,
        IPartRepository partRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _equipmentRepository = equipmentRepository;
        _partRepository = partRepository;
    }

    public async Task<ServiceOrderDto> CreateAsync(CreateServiceOrderRequest request)
    {
        // 1. Lógica de Validación: Verificar existencia de Cliente y Equipo [4, 5]
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new NotFoundException($"Cliente con ID {request.CustomerId} no encontrado.");

        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
        if (equipment == null)
            throw new NotFoundException($"Equipo con ID {request.EquipmentId} no encontrado.");

        // 2. Mapeo y Inicialización [4, 6]
        var order = new ServiceOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            EquipmentId = request.EquipmentId,
            FailureDescription = request.FailureDescription,
            // Regla: El estatus inicial siempre será Received (0) [4, 5]
            Status = ServiceOrderStatus.Received,
            // Cálculo: Inicialización de costo de mano de obra base (numeric 12,2) [6, 7]
            LaborCost = 0,
            ServiceTypeId = request.ServiceTypeId,
            // Regla: La fecha de creación es obligatoria y automática [4]
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // 3. Persistencia [6]
        await _orderRepository.AddAsync(order);

        // 4. Retorno del DTO con nombres cargados (aplanamiento para la UI) [8, 9]
        return new ServiceOrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = customer.Name,
            EquipmentId = order.EquipmentId,
            EquipmentTypeId = equipment.EquipmentTypeId,
            EquipmentTypeName = equipment.EquipmentTypeId.ToString(),
            EquipmentDescription = equipment.DescriptionType,
            EquipmentBrand = equipment.Brand,
            EquipmentModel = equipment.Model,
            FailureDescription = order.FailureDescription,
            Status = order.Status,
            ServiceTypeId= order.ServiceTypeId,
            ServiceTypeDescription = order.ServiceTypeId.ToString(),
            LaborCost = order.LaborCost,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }

    public async Task<ServiceOrderDto?> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return null;

        // Se asume que el repositorio ya hizo el .Include() para traer Customer y Equipment [9]
        return MapToDto(order);
    }

    public async Task<IEnumerable<ServiceOrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDto);
    }

    /// <summary>
    /// Consulta órdenes aplicando filtros opcionales (US 9).
    /// </summary>
    public async Task<IEnumerable<ServiceOrderDto>> GetAllFilteredAsync(ServiceOrderStatus? status, Guid? customerId)
    {
        var orders = await _orderRepository.GetAllFilteredAsync(status, customerId);
        return orders.Select(MapToDto);
    }
    /// <summary>
    /// Actualiza información general de la orden (US 16, 17).
    /// </summary>
    public async Task UpdateAsync(UpdateServiceOrderRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);

        if (order == null) throw new NotFoundException($"Orden con ID {request.Id} no encontrada.");

        // Regla 5.6: No se puede modificar una orden en estado Delivered [3]
        if (order.Status == ServiceOrderStatus.Delivered)
            throw new DomainException("No se pueden realizar cambios en una orden ya entregada.");

        // Regla 5.4: El costo de mano de obra no puede ser negativo [2, 5]
        if (request.LaborCost < 0)
            throw new DomainException("El costo de mano de obra debe ser mayor o igual a cero.");

        order.EquipmentId = request.EquipmentId;
        order.ServiceTypeId = request.ServiceTypeId; 
        order.FailureDescription = request.FailureDescription;
        order.LaborCost = request.LaborCost;
        order.UpdatedAt = DateTimeOffset.UtcNow; // Actualización de timestamptz [4]

        await _orderRepository.UpdateAsync(order);
    }
    /// <summary>
    /// Gestiona el cambio de estado siguiendo el flujo de trabajo (US 11).
    /// </summary>
    public async Task UpdateStatusAsync(UpdateServiceOrderStatusRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);

        if (order == null) throw new NotFoundException($"Orden con ID {request.Id} no encontrada.");

        // Regla 5.6: Bloqueo de edición en estado final [3]
        if (order.Status == ServiceOrderStatus.Delivered)
            throw new DomainException("El flujo ha finalizado; no se pueden cambiar estados de órdenes entregadas.");

        // Validación de Flujo (Regla 5.1):
        // 1. No se puede avanzar a Repairing sin haber pasado por Diagnosing [1]
        if (request.NewStatus == ServiceOrderStatus.Repairing && order.Status < ServiceOrderStatus.Diagnosing)
            throw new DomainException("Debe completar el diagnóstico antes de iniciar la reparación.");

        // 2. No se puede marcar como Delivered si no está en estado Completed [1]
        if (request.NewStatus == ServiceOrderStatus.Delivered && order.Status != ServiceOrderStatus.Completed)
            throw new DomainException("La orden debe marcarse como Completada antes de proceder a la Entrega.");
        await _orderRepository.UpdateStatusAsync(request.Id, request.NewStatus);
    }
    public async Task AddPartToOrderAsync(CreateOrderPartRequest request)
    {
        var orderPart = await _orderRepository.GetOrderPartAsync(request.ServiceOrderId, request.PartId);
        if (orderPart != null) throw new DomainException("La refacción ya está asignada a la orden. Use la opción de actualizar cantidad si desea modificarla.");
        
        // 1. Validar existencia y estado de la orden (Regla 5.6)
        var order = await _orderRepository.GetByIdAsync(request.ServiceOrderId);
        if (order == null) throw new NotFoundException($"Orden con ID {request.ServiceOrderId} no encontrada.");

        if (order.Status >= ServiceOrderStatus.Completed)
            throw new DomainException("No se pueden agregar refacciones a una orden completada o entregada.");

        // 2. Validar stock y obtener precio histórico (Regla 5.3)
        var part = await _partRepository.GetByIdAsync(request.PartId);
        if (part == null) throw new NotFoundException($"Refacción con ID {request.PartId} no encontrada.");

        int difference = request.Quantity - orderPart.Quantity;
        if (difference > 0 && part.Stock < difference)
            throw new DomainException("Stock insuficiente para realizar la asignación.");

        var newOrderPart = new OrderPart
        {
            ServiceOrderId = request.ServiceOrderId,
            PartId = request.PartId,
            Quantity = request.Quantity,
            UnitPrice = part.Price // Se guarda el precio actual como histórico
        };
        // 3. Persistir mediante repositorio (incluye descuento automático de stock)
        await _orderRepository.AddPartToOrderAsync(newOrderPart); 
    }

    public async Task UpdatePartToOrderAsync(UpdateOrderPartRequest request)
    {
        // 1. Validar estado de la orden (Regla 5.6)
        var order = await _orderRepository.GetByIdAsync(request.ServiceOrderId);
        if (order == null || order.Status >= ServiceOrderStatus.Completed)
            throw new NotFoundException($"La orden con ID {request.ServiceOrderId} no fue encontrada.");
        
        var orderPart = await _orderRepository.GetOrderPartAsync(request.ServiceOrderId, request.PartId);
        if (orderPart == null) throw new NotFoundException($"La pieza con ID {request.PartId} no está en la orden.");

        // 2. Validar stock adicional si la cantidad aumenta (Regla 5.3)
        // Nota: El repositorio se encargará del cálculo de la diferencia de stock.
        var part = await _partRepository.GetByIdAsync(request.PartId);
        if (part == null) throw new NotFoundException($"La refacción con ID {request.PartId} no fue encontrada.");

        int difference = request.Quantity - orderPart.Quantity;
        if (difference > 0 && part.Stock < difference) throw new InsufficientStockException();

        // Lógica de negocio simplificada: el repositorio ajustará el stock.
        await _orderRepository.UpdatePartToOrderAsync(
            request.ServiceOrderId,
            request.PartId,
            request.Quantity);
    }
    public async Task UpdateDiagnosisAsync(Guid id, string diagnosis)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new NotFoundException($"La orden con ID {id} no fue encontrada.");
        if (order.Status != ServiceOrderStatus.Diagnosing) throw new DomainException($"La actualizacion del diagnostico debe hacerde en la etapa de diagnostico");
        await _orderRepository.UpdateDiagnosisAsync(id, diagnosis); 
    }
    private static ServiceOrderDto MapToDto(ServiceOrder order) => new ServiceOrderDto
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        CustomerName = order.Customer?.Name ?? "N/A",
        EquipmentId = order.EquipmentId,
        EquipmentTypeId = order.Equipment?.EquipmentTypeId ?? EquipmentType.Unknown,
        EquipmentTypeName = order.Equipment?.EquipmentTypeId.ToString() ?? "N/A",
        EquipmentDescription = order.Equipment?.DescriptionType ?? "N/A",
        EquipmentBrand = order.Equipment?.Brand ?? "N/A",
        EquipmentModel = order.Equipment?.Model ?? "N/A",
        FailureDescription = order.FailureDescription,
        Status = order.Status,
        ServiceTypeId = order.ServiceTypeId,
        ServiceTypeDescription = order.ServiceTypeId.ToString(),
        LaborCost = order.LaborCost,
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt,
        OrderPart = order.OrderParts?.Select(op => new OrderPartDto
        {
            PartId = op.PartId,
            PartName = op.Part?.Name ?? "N/A", // Esto requiere el ThenInclude anterior
            Quantity = op.Quantity,
            UnitPrice = op.UnitPrice,
            Subtotal = op.Quantity * op.UnitPrice // Puedes agregar lógica simple aquí
        }).ToList() ?? new List<OrderPartDto>()
    };
}
