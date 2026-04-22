namespace WorkshopAdmin.Shared.Dtos.Equipments;

public class CreateEquipmentRequest
{
    public string Type { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? TechnicalSpecifications { get; set; } = string.Empty;
}
