using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Domain.Entities;

public class Part : BaseEntity
{
    public Guid Id { get; set; } // PK: uuid 
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty; // varchar(150) 
    public string Brand { get; set; } = string.Empty;
    public PartCategory PartCategoryId { get; set; } // Enum: Engine, Fluids, etc. [7]
    public decimal Price { get; set; } // numeric(12,2) 
    public int Stock { get; set; } // integer 
    public int MinStock { get; set; }
    public string UnitOfMeasure { get; set; } = "Pz";
    public string? WarehouseLocation { get; set; }
    public bool IsActive { get; set; } = true;

    // Propiedades de navegación
    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>(); // Relación 1:N 
    public virtual User CreatedByUser { get; set; } = null!; // Relación N:1 con User (quién creó o actualizó el registro)

}
