using WorkshopAdmin.Shared.Enums; // Referencia obligatoria a Shared 

namespace WorkshopAdmin.Domain.Entities;

public class ServiceOrder
{
    public Guid Id { get; set; } // PK: uuid 
    public Guid CustomerId { get; set; } // FK: uuid 
    public Guid EquipmentId { get; set; } // FK: uuid 
    public string FailureDescription { get; set; } = string.Empty; // text 
    public string? Diagnosis { get; set; } // Obligatorio antes de completar
    public ServiceOrderStatus Status { get; set; } // Enum (smallint) 
    public decimal LaborCost { get; set; } // numeric(12,2) 
    public DateTimeOffset CreatedAt { get; set; } // timestamptz 
    public DateTimeOffset UpdatedAt { get; set; } // timestamptz 

    // Propiedades de navegación
    public virtual Customer Customer { get; set; } = null!; // Relación 1:N 
    public virtual Equipment Equipment { get; set; } = null!; // Relación 1:N
    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>(); // Relación 1:N 
}