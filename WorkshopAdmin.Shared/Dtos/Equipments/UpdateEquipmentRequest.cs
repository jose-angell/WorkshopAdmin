namespace WorkshopAdmin.Shared.Dtos.Equipments;

public class UpdateEquipmentRequest
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}