namespace WorkshopAdmin.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; } // PK: uuid
    public string Name { get; set; } = string.Empty; // varchar(150) 
    public string Phone { get; set; } = string.Empty; // varchar(20) 
    public string Email { get; set; } = string.Empty; // varchar(150) 
    public bool IsActive { get; set; } = true; // boolean, default true
    public int CustomerNumber { get; private set; } // El número secuencial (ej: 1, 2, 3)
    public string FriendlyId { get; private set; } = string.Empty; // El código (ej: CUST-00001)
    public DateTimeOffset CreatedAt { get; set; } // timestamptz 

    // Propiedades de navegación
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>(); // Relación 1:N 
    public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>(); // Relación 1:N 
}
