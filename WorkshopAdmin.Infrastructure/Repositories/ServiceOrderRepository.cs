using Microsoft.EntityFrameworkCore;
using System.Collections;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Infrastructure.Persistence;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Infrastructure.Repositories;

public class ServiceOrderRepository : IServiceOrderRepository
{
    private readonly AppDbContext _context;

    public ServiceOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceOrder?> GetByIdAsync(Guid id)
    {
        return await _context.ServiceOrders
            .Include(so => so.Customer)   // Carga relacionada (Relación 1:N)
            .Include(so => so.Equipment)  // Carga relacionada (Relación 1:N)
            .Include(so => so.OrderParts) // Opcional: para cálculos de costos
            .ThenInclude(op => op.Part)
            .FirstOrDefaultAsync(so => so.Id == id);
    }

    public async Task<IEnumerable<ServiceOrder>> GetAllAsync()
    {
        return await _context.ServiceOrders
            .Include(so => so.Customer)
            .Include(so => so.Equipment)
            .Include(o => o.OrderParts) 
            .ThenInclude(op => op.Part)
            .OrderByDescending(so => so.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByStatusAsync(string status)
    {
        // Convertimos el string a Enum para comparar con el campo smallint de la DB
        if (Enum.TryParse<ServiceOrderStatus>(status, true, out var statusEnum))
        {
            return await _context.ServiceOrders
                .Include(so => so.Customer)
                .Include(so => so.Equipment)
                .Include(so => so.OrderParts)
                .ThenInclude(op => op.Part)
                .Where(so => so.Status == statusEnum)
                .ToListAsync();
        }
        return Enumerable.Empty<ServiceOrder>();
    }

    public async Task<IEnumerable<ServiceOrder>> GetByCustomerIdAsync(Guid customerId)
    {
        // Aprovecha el índice service_order(customer_id) definido en el Data Dictionary
        return await _context.ServiceOrders
            .Include(so => so.Equipment)
            .Include(o => o.OrderParts)
            .ThenInclude(op => op.Part)
            .Where(so => so.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task AddAsync(ServiceOrder serviceOrder)
    {
        await _context.ServiceOrders.AddAsync(serviceOrder);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ServiceOrder serviceOrder)
    {
        // Antes de actualizar, se podría validar la Regla 5.6: 
        // No modificar órdenes en estado Delivered
        serviceOrder.UpdatedAt = DateTimeOffset.UtcNow;
        _context.ServiceOrders.Update(serviceOrder);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await GetByIdAsync(id);
        if (order != null)
        {
            _context.ServiceOrders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<ServiceOrder>> GetAllFilteredAsync(ServiceOrderStatus? status, Guid? customerId)
    {
        var query = _context.ServiceOrders
            .Include(so => so.Customer)
            .Include(so => so.Equipment)
            .Include(o => o.OrderParts) 
            .ThenInclude(op => op.Part)
            .AsQueryable();

        // Aplicación de filtros opcionales
        if (status.HasValue)
        {
            query = query.Where(so => so.Status == status.Value);
        }

        if (customerId.HasValue)
        {
            query = query.Where(so => so.CustomerId == customerId.Value);
        }

        return await query
            .OrderByDescending(so => so.CreatedAt)
            .ToListAsync();
    }
    public async Task UpdateStatusAsync(Guid id, ServiceOrderStatus status)
    {
        var order = await _context.ServiceOrders.FindAsync(id);
        if (order != null)
        {
            order.Status = status;
            order.UpdatedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
    public async Task AddPartToOrderAsync(OrderPart orderPart)
    {
        // Descontar del inventario 
        var part = await _context.Parts.FindAsync(orderPart.PartId);
        if (part != null)
        {
            part.Stock -= orderPart.Quantity; // El sistema debe garantizar stock >= 0 previo a esto [5]
        }

        await _context.OrderParts.AddAsync(orderPart);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateDiagnosisAsync(Guid serviceOrderId, string diagnosis)
    {
        var orderService = await _context.ServiceOrders
            .FirstOrDefaultAsync(op => op.Id == serviceOrderId);

        if (orderService != null && orderService.Status == ServiceOrderStatus.Diagnosing)
        {
            orderService.Diagnosis = diagnosis;
            await _context.SaveChangesAsync();
        }
        
    }
    public async Task UpdatePartToOrderAsync(Guid serviceOrderId, Guid partId, int newQuantity)
    {
        // 1. Buscar la relación existente
        var orderPart = await _context.OrderParts
            .FirstOrDefaultAsync(op => op.ServiceOrderId == serviceOrderId && op.PartId == partId);

        if (orderPart != null)
        {
            var part = await _context.Parts.FindAsync(partId);
            if (part != null)
            {
                // 2. Ajustar el stock basado en la diferencia de cantidad
                int difference = newQuantity - orderPart.Quantity;
                part.Stock -= difference;
            }

            // 3. Actualizar cantidad (el precio histórico NO es editable según Regla 16) [5]
            orderPart.Quantity = newQuantity;

            await _context.SaveChangesAsync();
        }
    }
    public async Task<OrderPart?> GetOrderPartAsync(Guid serviceOrderId, Guid partId)
    {
        return await _context.OrderParts
            .Include(op => op.Part)
            .FirstOrDefaultAsync(op => op.ServiceOrderId == serviceOrderId && op.PartId == partId);
    }
    public async Task<IEnumerable<OrderPart?>> GetOrderPartByServiceAsync(Guid serviceOrderId)
    {
        return await _context.OrderParts
            .Include(op => op.Part)
            .Where(op => op.ServiceOrderId == serviceOrderId)
            .ToListAsync();
    }
}