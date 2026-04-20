namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class UpdateOrderPartRequest
{
    public Guid ServiceOrderId { get; set; }
    public Guid PartId { get; set; }
    public int Quantity { get; set; }
}