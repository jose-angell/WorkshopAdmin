using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text.Json;
using WorkshopAdmin.Shared.Dtos.Customers;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Dtos.Users;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.UI.Services
{
    public class UserClient
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _options;
        private const string BaseRoute = "api/user";

        public UserClient(HttpClient http)
        {
            _http = http;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        public async Task<List<UserDto>> GetAllAsync(UserRole? role)
        {
            var queryParams = new List<string>();

            if (role.HasValue)
                queryParams.Add($"role={(int)role.Value}");

            var url = queryParams.Any()
                ? $"{BaseRoute}?{string.Join("&", queryParams)}"
                : BaseRoute;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            try
            {
                return await _http.GetFromJsonAsync<List<UserDto>>(url, options) ?? new();
            }
            catch (Exception ex)
            {
                // Loguear el error o manejarlo según tu política
                return new List<UserDto>();
            }
        }
        /// <summary>
        /// Obtiene un usuario por su GUID
        /// </summary>
        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"{BaseRoute}/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserDto>(_options);
        }

        /// <summary>
        /// Obtiene un técnico por su GUID
        /// </summary>
        public async Task<UserDto?> GetTechnicianByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"{BaseRoute}/{id}/technician");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserDto>(_options);
        }

        /// <summary>
        /// Crear un usuario nuevo
        /// </summary>
        public async Task<bool> CreateAsync(CreateUserRequest order)
        {
            var response = await _http.PostAsJsonAsync(BaseRoute, order);
            return response.IsSuccessStatusCode;
        }
        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        public async Task<bool> UpdateAsync(UpdateUserRequest order)
        {
            var response = await _http.PutAsJsonAsync(BaseRoute, order);
            return response.IsSuccessStatusCode;
        }

    }
}
