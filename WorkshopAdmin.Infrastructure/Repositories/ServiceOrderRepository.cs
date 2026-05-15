using Microsoft.EntityFrameworkCore;
using System.Collections;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Exceptions;
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
            .AsNoTracking()
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
        var part = await _context.Parts
            .FirstOrDefaultAsync(p => p.Id == orderPart.PartId);

        if (part == null)
            throw new NotFoundException($"La refacción con ID {orderPart.PartId} no existe.");

        if (part.Stock < orderPart.Quantity)
            throw new InsufficientStockException();

        // Descontar del inventario 
        part.Stock -= orderPart.Quantity;

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
        var orderPart = await _context.OrderParts
         .FirstOrDefaultAsync(op => op.ServiceOrderId == serviceOrderId && op.PartId == partId)
         ?? throw new NotFoundException("Relación Orden-Pieza no encontrada.");

        var part = await _context.Parts.FindAsync(partId)
            ?? throw new NotFoundException("La refacción ya no existe en el catálogo.");

        int difference = newQuantity - orderPart.Quantity;

        if (part.Stock < difference)
            throw new InsufficientStockException();

        part.Stock -= difference;
        orderPart.Quantity = newQuantity;

        await _context.SaveChangesAsync();
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
    public async Task DeletePartToOrderAsync(Guid serviceOrderId, Guid partId)
    {
        var orderPart = await _context.OrderParts
        .FirstOrDefaultAsync(op => op.ServiceOrderId == serviceOrderId && op.PartId == partId)
        ?? throw new NotFoundException("Relación Orden-Pieza no encontrada.");

        var part = await _context.Parts.FindAsync(partId)
            ?? throw new NotFoundException("La refacción ya no existe en el catálogo.");

        part.Stock += orderPart.Quantity;

        _context.OrderParts.Remove(orderPart);
        await _context.SaveChangesAsync();

    }
    public async Task<int> CountCustomersWithOrdersAsync()
    {
        return await _context.ServiceOrders
            .Select(o => o.CustomerId)
            .Distinct()
            .CountAsync();
    }

    public async Task<int> CountReturningCustomersAsync()
    {
        return await _context.ServiceOrders
            .GroupBy(o => o.CustomerId)
            .CountAsync(g => g.Count() > 1);
    }
    public async Task<int> CountOrdersWithPartsAsync()
    {
        return await _context.ServiceOrders
            .CountAsync(o => o.OrderParts.Any());
    }

}