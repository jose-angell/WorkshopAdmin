using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.Equipments;

public class CreateEquipmentRequest
{
    public EquipmentType TypeId { get; set; } 
    public string DescriptionType { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? TechnicalSpecifications { get; set; } = string.Empty;
}
