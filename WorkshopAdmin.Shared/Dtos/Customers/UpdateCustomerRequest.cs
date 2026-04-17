namespace WorkshopAdmin.Shared.Dtos.Customers;


/// <summary>
/// DTO para la actualización de información existente (Requisito US 4.1).
/// Incluye el ID para identificar el registro y los campos editables.
/// </summary>
public class UpdateCustomerRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}