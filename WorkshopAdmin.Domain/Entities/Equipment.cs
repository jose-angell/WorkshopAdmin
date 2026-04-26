using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Domain.Entities;

public class Equipment
{
    public Guid Id { get; set; } // PK: uuid 
    public EquipmentType EquipmentTypeId { get; set; } // smallint en DB 
    public string DescriptionType { get; set; } = string.Empty; // varchar(100) Subtipo/Descripción:
    public string Brand { get; set; } = string.Empty; // varchar(100) 
    public string Model { get; set; } = string.Empty; // varchar(100) 
    public string? TechnicalSpecifications { get; set; } = string.Empty; // varchar(2000) 
    public bool IsActive { get; set; } = true; // Estado lógico [2]
    public DateTimeOffset CreatedAt { get; set; } // timestamptz 

    // Propiedades de navegación
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>(); // Relación 1:N 
}
