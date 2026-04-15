namespace WorkshopAdmin.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; } // PK: uuid
    public string Name { get; set; } = string.Empty; // varchar(150) 
    public string Phone { get; set; } = string.Empty; // varchar(20) 
    public string Email { get; set; } = string.Empty; // varchar(150) 
    public DateTimeOffset CreatedAt { get; set; } // timestamptz 

    // Propiedades de navegación
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>(); // Relación 1:N 
}
