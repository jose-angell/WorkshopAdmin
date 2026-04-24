namespace WorkshopAdmin.Shared.Dtos.Customers;


/// <summary>
/// DTO para la visualización completa de un cliente.
/// Mapea directamente con la entidad Customer del Data Dictionary.
/// </summary>
public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public int ActiveOrders { get; set; } = 0;
    public DateTimeOffset CreatedAt { get; set; }
}