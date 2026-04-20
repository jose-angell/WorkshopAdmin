using System.Collections;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Shared.Enums; // Para el manejo de estados

namespace WorkshopAdmin.Domain.Interfaces;

public interface IServiceOrderRepository
{
    Task<ServiceOrder?> GetByIdAsync(Guid id);
    Task<IEnumerable<ServiceOrder>> GetAllAsync();
    Task AddAsync(ServiceOrder serviceOrder);
    Task UpdateAsync(ServiceOrder serviceOrder);
    Task DeleteAsync(Guid id);
    Task AddPartToOrderAsync(OrderPart orderPart);
    Task UpdatePartToOrderAsync(Guid serviceOrderId, Guid partId, int newQuantity);
    // Métodos específicos solicitados
    Task<IEnumerable<ServiceOrder>> GetByStatusAsync(string status);
    Task<IEnumerable<ServiceOrder>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<ServiceOrder>> GetAllFilteredAsync(ServiceOrderStatus? status, Guid? customerId);
    Task UpdateStatusAsync(Guid id, ServiceOrderStatus status);
    Task<OrderPart?> GetOrderPartAsync(Guid serviceOrderId, Guid partId);
    Task<IEnumerable<OrderPart?>> GetOrderPartByServiceAsync(Guid serviceOrderId);
}
