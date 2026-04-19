namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class UpdateServiceOrderRequest
{
    public Guid Id { get; set; } // PK: uuid
    public string FailureDescription { get; set; } = string.Empty;
    public decimal LaborCost { get; set; } // numeric(12,2)
}