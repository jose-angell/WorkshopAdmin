using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class UpdateServiceOrderTechnicalDataRequest
{
    public Guid Id { get; set; } // PK: uuid
    public string FailureDescription { get; set; } = string.Empty;
    public string? Diagnosis { get; set; } 
    public decimal LaborCost { get; set; } // numeric(12,2)
    public ServiceOrderStatus NewStatus { get; set; }

}