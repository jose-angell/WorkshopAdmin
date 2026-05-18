using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Application.Interfaces;

public interface IServiceOrderService
{
    Task<ServiceOrderDto> CreateAsync(CreateServiceOrderRequest request);
    Task<ServiceOrderDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ServiceOrderDto>> GetAllAsync();
    Task<IEnumerable<ServiceOrderDto>> GetAllFilteredAsync(ServiceOrderStatus? status, Guid? customerId);
    Task UpdateStatusAsync(UpdateServiceOrderStatusRequest request);
    Task UpdateAsync(UpdateServiceOrderRequest request);
    Task UpdateDiagnosisAsync(Guid id, string diagnosis);
    Task<OrderPartDto?> GetOrderPartAsync(Guid serviceOrderId, Guid partId);
    Task AddPartToOrderAsync(CreateOrderPartRequest request);
    Task UpdatePartToOrderAsync(UpdateOrderPartRequest request);
    Task DeletePartToOrderAsync(Guid serviceOrderId, Guid partId);
    Task UpdateTechnicalDataAsync(UpdateServiceOrderTechnicalDataRequest request);
    Task<ServiceOrderStatsDto?> GetStatsAsync();
    Task<IEnumerable<ServiceOrderDto>> GetOrdersByCustomerIdAsync(Guid customerId);
}