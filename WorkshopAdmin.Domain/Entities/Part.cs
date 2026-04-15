namespace WorkshopAdmin.Domain.Entities;

public class Part
{
    public Guid Id { get; set; } // PK: uuid 
    public string Name { get; set; } = string.Empty; // varchar(150) 
    public decimal Price { get; set; } // numeric(12,2) 
    public int Stock { get; set; } // integer 
    public DateTimeOffset CreatedAt { get; set; } // timestamptz 

    // Propiedades de navegación
    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>(); // Relación 1:N 
}
