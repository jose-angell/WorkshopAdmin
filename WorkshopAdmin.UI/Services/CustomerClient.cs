using System.Net.Http.Json;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Customers;

namespace WorkshopAdmin.UI.Services;

public class CustomerClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _options;
    private const string BaseRoute = "api/customers";

    public CustomerClient(HttpClient http)
    {
        _http = http;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    /// <summary>
    /// Obtiene todos los clientes
    /// </summary>
    public async Task<List<CustomerDto>> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync(BaseRoute);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CustomerDto>>(_options) ?? new();
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
    /// Obtiene un cliente por su GUID
    /// </summary>
    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var response = await _http.GetAsync($"{BaseRoute}/{id}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<CustomerDto>(_options);
    }

    /// <summary>
    /// Crea un nuevo cliente (POST)
    /// </summary>
    public async Task<CustomerDto?> CreateAsync(CreateCustomerRequest request)
    {
        var response = await _http.PostAsJsonAsync(BaseRoute, request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CustomerDto>(_options);
        }

        return null;
    }

    /// <summary>
    /// Actualiza los datos de un cliente (PUT)
    /// </summary>
    public async Task<bool> UpdateAsync(UpdateCustomerRequest request)
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
    /// Elimina un cliente (DELETE)
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"{BaseRoute}/{id}");
        return response.IsSuccessStatusCode;
    }
}

