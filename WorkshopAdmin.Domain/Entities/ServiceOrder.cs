using WorkshopAdmin.Shared.Enums; // Referencia obligatoria a Shared 

namespace WorkshopAdmin.Domain.Entities;

public class ServiceOrder: BaseEntity
{
    public Guid Id { get; set; } // PK: uuid 
    public Guid CustomerId { get; set; } // FK: uuid 
    public Guid EquipmentId { get; set; } // FK: uuid 
    public string FailureDescription { get; set; } = string.Empty; // text 
    public string? Diagnosis { get; set; } // Obligatorio antes de completar
    public ServiceOrderStatus Status { get; set; } // Enum (smallint) 
    public decimal LaborCost { get; set; } // numeric(12,2) 
    public TimeSpan? EstimatedTime { get; set; } // interval
    public DateTimeOffset? RepairStartedAt { get; set; } // timestamptz
    public DateTimeOffset? RepairFinishedAt { get; set; } // timestamptz
    public DateTimeOffset? ExpectedFinishAt { get; set; } // timestamptz
    public ServiceType ServiceTypeId { get; set; } // Enum: Preventive, Corrective, etc. [8]
    public int OrderNumber { get; private set; } // El número secuencial (ej: 1, 2, 3)
    public string FriendlyId { get; private set; } = string.Empty; // El código (ej: ORD-00001)
    public Guid TechnicianId { get; set; } // FK: uuid, el técnico asignado a la orden de servicio

    // Propiedades de navegación
    public virtual Customer Customer { get; set; } = null!; // Relación 1:N 
    public virtual Equipment Equipment { get; set; } = null!; // Relación 1:N
    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>(); // Relación 1:N 
    public virtual User CreatedByUser { get; set; } = null!;
    public virtual User Technician { get; set; } = null!;
}