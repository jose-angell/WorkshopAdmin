namespace WorkshopAdmin.Shared.Dtos.Customers;

public class CustomerStatsDto
{
    public int NewClientsThisMonth { get; set; }

    public decimal NewClientsGrowthPercentage { get; set; }

    public decimal ServiceRetentionRate { get; set; }
}