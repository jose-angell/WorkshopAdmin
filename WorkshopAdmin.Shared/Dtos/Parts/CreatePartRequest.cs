using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.Parts;

public class CreatePartRequest
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public PartCategory CategoryId { get; set; }

    // Regla de Integridad: unit_price >= 0
    public decimal Price { get; set; }

    // Regla de Integridad: stock >= 0
    public int Stock { get; set; }
    public int MinStock { get; set; }

    public string UnitOfMeasure { get; set; } = "Pz";
    public string? WarehouseLocation { get; set; }
}