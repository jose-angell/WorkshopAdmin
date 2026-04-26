using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.Parts;

public class PartDto
{
    public Guid Id { get; set; } // uuid (PK)
    public string Sku { get; set; } = string.Empty; // Código único / fabricante
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;

    // Categoría según el catálogo definido (smallint)
    public PartCategory CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty; // Para visualización amigable

    public decimal Price { get; set; } // numeric(12,2)
    public int Stock { get; set; }
    public int MinStock { get; set; } // Punto de reorden
    public string UnitOfMeasure { get; set; } = "Pz";
    public string? WarehouseLocation { get; set; }

    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; } // timestamptz
}