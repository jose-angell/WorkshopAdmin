using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class UpdateServiceOrderRequest
{
    public Guid Id { get; set; } // PK: uuid
    public Guid EquipmentId { get; set; }
    public string FailureDescription { get; set; } = string.Empty;
    public string? Diagnosis { get; set; } 
    public ServiceType ServiceTypeId { get; set; }
    public decimal LaborCost { get; set; } // numeric(12,2)
}