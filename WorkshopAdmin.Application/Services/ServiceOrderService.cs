using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Application.Services;

public class ServiceOrderService : IServiceOrderService
{
    private readonly IServiceOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public ServiceOrderService(
        IServiceOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IEquipmentRepository equipmentRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ServiceOrderDto> CreateAsync(CreateServiceOrderRequest request)
    {
        // 1. Lógica de Validación: Verificar existencia de Cliente y Equipo [4, 5]
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new ApplicationException("El cliente proporcionado no es válido o no existe.");

        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
        if (equipment == null)
            throw new ApplicationException("El equipo proporcionado no está registrado en el sistema.");

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
            EquipmentType = equipment.Type,
            EquipmentBrand = equipment.Brand,
            EquipmentModel = equipment.Model,
            FailureDescription = order.FailureDescription,
            Status = order.Status,
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

        if (order == null) throw new ApplicationException("Orden no encontrada.");

        // Regla 5.6: No se puede modificar una orden en estado Delivered [3]
        if (order.Status == ServiceOrderStatus.Delivered)
            throw new ApplicationException("No se pueden realizar cambios en una orden ya entregada.");

        // Regla 5.4: El costo de mano de obra no puede ser negativo [2, 5]
        if (request.LaborCost < 0)
            throw new ApplicationException("El costo de mano de obra debe ser mayor o igual a cero.");

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

        if (order == null) throw new ApplicationException("Orden no encontrada.");

        // Regla 5.6: Bloqueo de edición en estado final [3]
        if (order.Status == ServiceOrderStatus.Delivered)
            throw new ApplicationException("El flujo ha finalizado; no se pueden cambiar estados de órdenes entregadas.");

        // Validación de Flujo (Regla 5.1):
        // 1. No se puede avanzar a Repairing sin haber pasado por Diagnosing [1]
        if (request.NewStatus == ServiceOrderStatus.Repairing && order.Status < ServiceOrderStatus.Diagnosing)
            throw new ApplicationException("Debe completar el diagnóstico antes de iniciar la reparación.");

        // 2. No se puede marcar como Delivered si no está en estado Completed [1]
        if (request.NewStatus == ServiceOrderStatus.Delivered && order.Status != ServiceOrderStatus.Completed)
            throw new ApplicationException("La orden debe marcarse como Completada antes de proceder a la Entrega.");

        await _orderRepository.UpdateStatusAsync(request.Id, request.NewStatus);
    }
    private static ServiceOrderDto MapToDto(ServiceOrder order) => new ServiceOrderDto
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        CustomerName = order.Customer?.Name ?? "N/A",
        EquipmentId = order.EquipmentId,
        EquipmentType = order.Equipment?.Type ?? "N/A",
        EquipmentBrand = order.Equipment?.Brand ?? "N/A",
        EquipmentModel = order.Equipment?.Model ?? "N/A",
        FailureDescription = order.FailureDescription,
        Status = order.Status,
        LaborCost = order.LaborCost,
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt
    };
}
