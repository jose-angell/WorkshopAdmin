using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.Equipments;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public string FriendlyId { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerFriendlyId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public EquipmentType TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string DescriptionType { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? TechnicalSpecifications { get; set; } = string.Empty;
    public bool IsActive { get; set; }= true;
    public Guid CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } // timestamptz
    public DateTimeOffset? UpdatedAt { get; set; }
}