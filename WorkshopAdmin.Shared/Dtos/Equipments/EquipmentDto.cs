using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.Equipments;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public EquipmentType TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string DescriptionType { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? TechnicalSpecifications { get; set; } = string.Empty;
    public bool IsActive { get; set; }= true;
    public DateTimeOffset CreatedAt { get; set; }
}