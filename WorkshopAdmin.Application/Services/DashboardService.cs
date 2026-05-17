using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Dashboard;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IServiceOrderRepository _orderRepository;
    private readonly IPartRepository _partRepository;

    public DashboardService(
        IServiceOrderRepository orderRepository,
        IPartRepository partRepository)
    {
        _orderRepository = orderRepository;
        _partRepository = partRepository;
    }

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var now = DateTimeOffset.UtcNow;

        var todayStart = new DateTimeOffset(
            now.Year,
            now.Month,
            now.Day,
            0, 0, 0,
            TimeSpan.Zero);

        var yesterdayStart = todayStart.AddDays(-1);

        // =====================================
        // ACTIVE ORDERS
        // =====================================

        var activeOrders =
            await _orderRepository.CountActiveOrdersAsync();

        var pendingApprovals =
            await _orderRepository.CountPendingDiagnosticsAsync();

        var activeToday =
            await _orderRepository.CountCreatedSinceAsync(todayStart);

        var activeYesterday =
            await _orderRepository.CountCreatedBetweenAsync(
                yesterdayStart,
                todayStart);

        decimal activeGrowth = 0;

        if (activeYesterday > 0)
        {
            activeGrowth =
                ((decimal)(activeToday - activeYesterday)
                / activeYesterday) * 100;
        }

        // =====================================
        // LOW STOCK
        // =====================================

        var lowStock =
            await _partRepository.CountLowStockAsync();

        var criticalPart =
            await _partRepository.GetMostCriticalLowStockPartAsync();

        // =====================================
        // DAILY REVENUE
        // =====================================

        var dailyRevenue =
            await _orderRepository.GetDailyRevenueAsync(todayStart);

        var yesterdayRevenue =
            await _orderRepository.GetDailyRevenueAsync(yesterdayStart);

        decimal revenueGrowth = 0;

        if (yesterdayRevenue > 0)
        {
            revenueGrowth =
                ((dailyRevenue - yesterdayRevenue)
                / yesterdayRevenue) * 100;
        }

        var averageTicket =
            await _orderRepository.GetAverageTicketAsync();

        return new DashboardStatsDto
        {
            ActiveOrders = activeOrders,
            PendingApprovals = pendingApprovals,
            ActiveOrdersGrowthPercentage = Math.Round(activeGrowth, 1),

            LowStockParts = lowStock,
            CriticalPartName = criticalPart,

            DailyRevenue = dailyRevenue,
            AverageTicket = averageTicket,
            DailyRevenueGrowthPercentage = Math.Round(revenueGrowth, 1)
        };
    }
    public async Task<ServiceVolumeTrendDto> GetServiceVolumeTrendAsync()
    {
        var now = DateTimeOffset.UtcNow;

        var today = new DateTimeOffset(
            now.Year,
            now.Month,
            now.Day,
            0, 0, 0,
            TimeSpan.Zero);

        var result = new ServiceVolumeTrendDto();

        for (int i = 6; i >= 0; i--)
        {
            var day = today.AddDays(-i);

            var intake =
                await _orderRepository.CountCreatedByDayAsync(day);

            var completed =
                await _orderRepository.CountCompletedByDayAsync(day);

            result.Points.Add(new ServiceVolumePointDto
            {
                Label = day.ToString("ddd"),
                Intake = intake,
                Completed = completed
            });
        }

        return result;
    }
}