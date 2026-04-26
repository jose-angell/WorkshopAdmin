using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.Parts;

public class UpdatePartRequest
{
    public Guid Id { get; set; } // Obligatorio para identificar el registro
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public PartCategory CategoryId { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; }

    public string UnitOfMeasure { get; set; } = "Pz";
    public string? WarehouseLocation { get; set; }
    public bool IsActive { get; set; }
}