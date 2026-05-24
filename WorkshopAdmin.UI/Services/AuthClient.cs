using System.Net.Http.Json;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Auth; // Asegúrate de tener aquí tus DTOs de login

namespace WorkshopAdmin.UI.Services;

public class AuthClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _options;
    private const string BaseRoute = "api/auth"; 

    public AuthClient(HttpClient http)
    {
        _http = http;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Envía las credenciales al servidor y obtiene la respuesta con el token
    /// </summary>
    public async Task<LoginDto?> LoginAsync(LoginRequest loginDto)
    {
        var response = await _http.PostAsJsonAsync($"{BaseRoute}/login", loginDto);

        if (!response.IsSuccessStatusCode)
        {
            return null; 
        }

        return await response.Content.ReadFromJsonAsync<LoginDto>(_options);
    }
}