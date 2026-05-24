using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace WorkshopAdmin.UI.Services;

public class AuthService(AuthenticationStateProvider authenticationStateProvider)
{
    private readonly AuthenticationStateProvider _authStateProvider = authenticationStateProvider;

    public async Task<bool> IsAuthenticated()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string> GetUsername()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    }

    public async Task<string> GetRole()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
    }
    public async Task<string> GetFullName()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst(c => c.Type == "unique_name")?.Value ?? "Usuario";
    }
}
