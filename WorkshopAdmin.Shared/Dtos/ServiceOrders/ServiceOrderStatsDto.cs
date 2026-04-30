namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class ServiceOrderStatsDto
{
    public int ActiveOrders { get; set; }
    public int NewOrdersThisWeek { get; set; }
    public int PendingDiagnostics { get; set; }
    public int AvgDiagnosticHours { get; set; }
    public decimal TotalRepairCost { get; set; }
}