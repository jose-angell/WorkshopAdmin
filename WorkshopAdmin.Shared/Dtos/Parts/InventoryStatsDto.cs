namespace WorkshopAdmin.Shared.Dtos.Parts;

public class InventoryStatsDto
{
    public int TotalParts { get; set; }

    public decimal TotalPartsGrowthPercentage { get; set; }

    public int LowStockAlerts { get; set; }

    public decimal InventoryValue { get; set; }

    public int OrdersUsingParts { get; set; }
}
