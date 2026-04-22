namespace WorkshopAdmin.Domain.Entities;

public class Equipment
{
    public Guid Id { get; set; } // PK: uuid 
    public string Type { get; set; } = string.Empty; // varchar(50) 
    public string Brand { get; set; } = string.Empty; // varchar(100) 
    public string Model { get; set; } = string.Empty; // varchar(100) 
    public string? TechnicalSpecifications { get; set; } = string.Empty; // varchar(2000) 
    public DateTimeOffset CreatedAt { get; set; } // timestamptz 

    // Propiedades de navegación
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>(); // Relación 1:N 
}
