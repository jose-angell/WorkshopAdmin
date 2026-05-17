namespace WorkshopAdmin.Shared.Dtos.Dashboard;

public class DashboardStatsDto
{
    // ACTIVE ORDERS
    public int ActiveOrders { get; set; }
    public int PendingApprovals { get; set; }
    public decimal ActiveOrdersGrowthPercentage { get; set; }

    // LOW STOCK
    public int LowStockParts { get; set; }
    public string? CriticalPartName { get; set; }

    // DAILY REVENUE
    public decimal DailyRevenue { get; set; }
    public decimal AverageTicket { get; set; }
    public decimal DailyRevenueGrowthPercentage { get; set; }
}