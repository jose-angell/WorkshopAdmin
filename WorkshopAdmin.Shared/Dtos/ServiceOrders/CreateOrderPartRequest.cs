namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class CreateOrderPartRequest
{
    public Guid ServiceOrderId { get; set; }
    public Guid PartId { get; set; }
    public int Quantity { get; set; }

    // Representa el precio histórico (numeric 12,2) al momento de la asignación [4, 6]
    public decimal UnitPrice { get; set; }
}