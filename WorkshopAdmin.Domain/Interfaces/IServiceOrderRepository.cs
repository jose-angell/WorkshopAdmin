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
    Task DeletePartToOrderAsync(Guid serviceOrderId, Guid partId);
    Task AddPartToOrderAsync(OrderPart orderPart);
    Task UpdateDiagnosisAsync(Guid serviceOrderId, string diagnosis);
    Task UpdatePartToOrderAsync(Guid serviceOrderId, Guid partId, int newQuantity);
    // Métodos específicos solicitados
    Task<IEnumerable<ServiceOrder>> GetByStatusAsync(string status);
    Task<IEnumerable<ServiceOrder>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<ServiceOrder>> GetByEquipmentIdAsync(Guid equipmentId);
    Task<IEnumerable<ServiceOrder>> GetAllFilteredAsync(ServiceOrderStatus? status, Guid? customerId);
    Task UpdateStatusAsync(Guid id, ServiceOrderStatus status);
    Task<OrderPart?> GetOrderPartAsync(Guid serviceOrderId, Guid partId);
    Task<IEnumerable<OrderPart?>> GetOrderPartByServiceAsync(Guid serviceOrderId);
    Task<int> CountCustomersWithOrdersAsync();
    Task<int> CountReturningCustomersAsync();
    Task<int> CountOrdersWithPartsAsync();
    Task<int> CountActiveOrdersAsync();
    Task<int> CountPendingDiagnosticsAsync();
    Task<int> CountCreatedSinceAsync(DateTimeOffset date);
    Task<int> CountCreatedBetweenAsync( DateTimeOffset start, DateTimeOffset end);
    Task<decimal> GetDailyRevenueAsync(DateTimeOffset day);
    Task<decimal> GetAverageTicketAsync();
    Task<int> CountCreatedByDayAsync(DateTimeOffset day);
    Task<int> CountCompletedByDayAsync(DateTimeOffset day);
}
