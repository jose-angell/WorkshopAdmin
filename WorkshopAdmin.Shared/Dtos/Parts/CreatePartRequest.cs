namespace WorkshopAdmin.Shared.Dtos.Parts;

public class CreatePartRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}