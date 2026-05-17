namespace WorkshopAdmin.Shared.Dtos.Dashboard;

public class LowStockPartDto
{
    public Guid Id { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int CurrentStock { get; set; }

    public int MinimumStock { get; set; }

    public string Category { get; set; } = string.Empty;

    public string? WarehouseLocation { get; set; }

    public bool IsCritical => CurrentStock <= 0;
}