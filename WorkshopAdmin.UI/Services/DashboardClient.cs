using System.Net.Http.Json;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Dashboard;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.UI.Services;

public class DashboardClient
{
    private readonly HttpClient _http;
    private const string BaseRoute = "api/dashboard";

    public DashboardClient(HttpClient http) => _http = http;

    /// <summary>
    /// Consultar todas las órdenes de servicio
    /// </summary>
    public async Task<List<ServiceOrderDto>> GetAllAsync(ServiceOrderStatus? status = null, Guid? customerId = null)
    {
        var queryParams = new List<string>();

        // Agregamos los parámetros solo si el usuario los proporcionó
        if (status.HasValue)
            queryParams.Add($"status={(int)status.Value}");

        if (customerId.HasValue)
            queryParams.Add($"customerId={customerId.Value}");

        // Construimos la URL final
        var url = queryParams.Any()
            ? $"{BaseRoute}?{string.Join("&", queryParams)}"
            : BaseRoute;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        try
        {
            return await _http.GetFromJsonAsync<List<ServiceOrderDto>>(url, options) ?? new();
        }
        catch (Exception ex)
        {
            // Loguear el error o manejarlo según tu política
            return new List<ServiceOrderDto>();
        }
    }
    /// <summary>
    /// Consulta las estadísticas de las órdenes de servicio (total, por estado, retrasadas, etc.)
    /// </summary>
    public async Task<DashboardStatsDto?> GetStatsAsync() =>
        await _http.GetFromJsonAsync<DashboardStatsDto>($"{BaseRoute}/stats");

    /// <summary>
    /// Obtiene la tendencia del volumen de servicios
    /// </summary>
    public async Task<ServiceVolumeTrendDto?> GetServiceVolumeTrendAsync() =>
        await _http.GetFromJsonAsync<ServiceVolumeTrendDto>($"{BaseRoute}/service-volume-trend");

    /// <summary>
    /// Obtiene el listado de partes con stock bajo
    /// </summary>
    public async Task<List<LowStockPartDto>?> GetLowStockPartsAsync() =>
        await _http.GetFromJsonAsync<List<LowStockPartDto>>($"{BaseRoute}/low-stock") ?? new();

    /// <summary>
    /// Consulta una orden de servicio por su ID
    /// </summary>
    public async Task<ServiceOrderDto?> GetByIdAsync(Guid id) =>
        await _http.GetFromJsonAsync<ServiceOrderDto>($"{BaseRoute}/{id}");

    
    /// <summary>
    /// Actualiza el estatus de la orden de servicio
    /// </summary>
    public async Task<bool> UpdateStatusAsync(UpdateServiceOrderStatusRequest request)
    {
        var response = await _http.PatchAsJsonAsync($"{BaseRoute}/{request.Id}/status", request);
        return response.IsSuccessStatusCode;
    }
}
