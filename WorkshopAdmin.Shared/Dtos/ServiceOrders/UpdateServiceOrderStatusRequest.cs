using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class UpdateServiceOrderStatusRequest
{
    public Guid Id { get; set; }
    public ServiceOrderStatus NewStatus { get; set; }

}