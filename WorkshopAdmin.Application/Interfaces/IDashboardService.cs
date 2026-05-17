using WorkshopAdmin.Shared.Dtos.Dashboard;

namespace WorkshopAdmin.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetStatsAsync();
    Task<ServiceVolumeTrendDto> GetServiceVolumeTrendAsync();
    Task<IEnumerable<LowStockPartDto>> GetLowStockPartsAsync();
}