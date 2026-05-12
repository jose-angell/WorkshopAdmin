using System.Net.Http.Json;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Customers;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.UI.Services;

public class ServiceOrderClient
{
    private readonly HttpClient _http;
    private const string BaseRoute = "api/service-orders";

    public ServiceOrderClient(HttpClient http) => _http = http;

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
    /// Consulta una orden de servicio por su ID
    /// </summary>
    public async Task<ServiceOrderDto?> GetByIdAsync(Guid id) =>
        await _http.GetFromJsonAsync<ServiceOrderDto>($"{BaseRoute}/{id}");

    /// <summary>
    /// Crear una nueva orden de servicio
    /// </summary>
    public async Task<bool> CreateAsync(CreateServiceOrderRequest order)
    {
        var response = await _http.PostAsJsonAsync(BaseRoute, order);
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Actualiza una orden de servicio existente
    /// </summary>
    public async Task<bool> UpdateAsync(UpdateServiceOrderRequest order)
    {
        var response = await _http.PutAsJsonAsync(BaseRoute, order);
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Actualiza el estatus de la orden de servicio
    /// </summary>
    public async Task<bool> UpdateStatusAsync(UpdateServiceOrderStatusRequest request)
    {
        var response = await _http.PatchAsJsonAsync($"{BaseRoute}/{request.Id}/status", request);
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Actualiza los datos técnicos de la orden de servicio
    /// </summary>
    public async Task<bool> UpdateTechnicalDataAsync(UpdateServiceOrderTechnicalDataRequest request)
    {
        var response = await _http.PatchAsJsonAsync($"{BaseRoute}/{request.Id}/technical-data", request);
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Consulta una orden de parte por su ID
    /// </summary>
    public async Task<OrderPartDto?> GetOrderPartByIdAsync(Guid serviceOrderId, Guid partId) =>
        await _http.GetFromJsonAsync<OrderPartDto>($"{BaseRoute}/{serviceOrderId}/parts/{partId}");


    /// <summary>
    /// Crea una nueva parte de la orden de servicio
    /// </summary>
    public async Task<bool> CreateOrderPartAsync(CreateOrderPartRequest order)
    {
        var response = await _http.PostAsJsonAsync($"{BaseRoute}/parts", order);
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Actualiza una parte de la orden de servicio
    /// </summary>
    public async Task<bool> UpdateOrderPartAsync(UpdateOrderPartRequest order)
    {
        var response = await _http.PutAsJsonAsync($"{BaseRoute}/parts", order);
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Elimina una parte de la orden de servicio
    /// </summary>
    public async Task<bool> DeleteOrderPartAsync(Guid serviceOrderId, Guid partId)
    {
        var response = await _http.DeleteAsync($"{BaseRoute}/{serviceOrderId}/parts/{partId}");
        return response.IsSuccessStatusCode;
    }
}
