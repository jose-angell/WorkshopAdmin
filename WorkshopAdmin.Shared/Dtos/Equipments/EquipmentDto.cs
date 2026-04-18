namespace WorkshopAdmin.Shared.Dtos.Equipments;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}