using System.Net.Http.Json;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Parts;

namespace WorkshopAdmin.UI.Services;
public class InventoryClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _options;
    private const string BaseRoute = "api/inventory";

    public InventoryClient(HttpClient http)
    {
        _http = http;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    /// <summary>
    /// Obtiene todos las refacciones del inventario
    /// </summary>
    public async Task<List<PartDto>> GetAllAsync()
    {
        var response = await _http.GetAsync(BaseRoute);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error fetching inventory: {error}");
        }

        return await response.Content.ReadFromJsonAsync<List<PartDto>>(_options) ?? new();
    }
    /// <summary>
    /// Obtiene una refacción por su GUID
    /// </summary>
    public async Task<PartDto?> GetByIdAsync(Guid id)
    {
        var response = await _http.GetAsync($"{BaseRoute}/{id}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<PartDto>(_options);
    }
    /// <summary>
    /// Crea una nueva refaccion (POST)
    /// </summary>
    public async Task<PartDto?> CreateAsync(CreatePartRequest request)
    {
        var response = await _http.PostAsJsonAsync(BaseRoute, request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PartDto>(_options);
        }

        return null;
    }

    /// <summary>
    /// Actualiza los datos de una refacción (PUT)
    /// </summary>
    public async Task<bool> UpdateAsync(UpdatePartRequest request)
    {
        var response = await _http.PutAsJsonAsync(BaseRoute, request);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Cambia el estado Activo/Inactivo (PUT a ruta específica)
    /// </summary>
    public async Task<bool> UpdateStatusAsync(Guid id)
    {
        // Enviamos null como body porque el controlador solo espera el ID en la URL
        var response = await _http.PutAsync($"{BaseRoute}/status/{id}", null); ;
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Elimina una refacción (DELETE)
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"{BaseRoute}/{id}");
        return response.IsSuccessStatusCode;
    }
}

