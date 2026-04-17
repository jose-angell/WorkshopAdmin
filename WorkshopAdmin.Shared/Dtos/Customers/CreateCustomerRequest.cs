namespace WorkshopAdmin.Shared.Dtos.Customers;

/// <summary>
/// DTO para la creación de un nuevo cliente (Requisito US 4.1).
/// No incluye ID ni CreatedAt ya que se generan automáticamente en el backend.
/// </summary>
public class CreateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
