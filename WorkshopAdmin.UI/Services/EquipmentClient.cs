using System.Net.Http.Json;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Customers;
using WorkshopAdmin.Shared.Dtos.Equipments;

namespace WorkshopAdmin.UI.Services;
public class EquipmentClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _options;
    private const string BaseRoute = "api/equipment";
    public EquipmentClient(HttpClient http)
    {
        _http = http;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    /// <summary>
    /// Obtiene todos los equipos registrados (US 5).
    /// </summary>
    public async Task<List<EquipmentDto>> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync(BaseRoute);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<EquipmentDto>>(_options) ?? new();
            }
            return new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetAllAsync: {ex.Message}");
            return new();
        }
    }
    /// <summary>
    /// Obtiene un equipo por su GUID
    /// </summary>
    public async Task<EquipmentDto?> GetByIdAsync(Guid id)
    {
        var response = await _http.GetAsync($"{BaseRoute}/{id}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<EquipmentDto>(_options);
    }
    /// <summary>
    /// Crea un nuevo equipo (POST)
    /// </summary>
    public async Task<EquipmentDto?> CreateAsync(CreateEquipmentRequest request)
    {
        var response = await _http.PostAsJsonAsync(BaseRoute, request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EquipmentDto>(_options);
        }

        return null;
    }
    /// <summary>
    /// Actualiza los datos de un equipo (PUT)
    /// </summary>
    public async Task<bool> UpdateAsync(UpdateEquipmentRequest request)
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
        var response = await _http.PutAsJsonAsync($"{BaseRoute}/status/{id}", new { });
        return response.IsSuccessStatusCode;
    }
    /// <summary>
    /// Elimina un equipo (DELETE)
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"{BaseRoute}/{id}");
        return response.IsSuccessStatusCode;
    }
}

