using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class CreateServiceOrderRequest
{
    public Guid CustomerId { get; set; } // Obligatorio
    public Guid EquipmentId { get; set; } // Obligatorio
    public string FailureDescription { get; set; } = string.Empty;
    public ServiceType ServiceTypeId { get; set; }
}