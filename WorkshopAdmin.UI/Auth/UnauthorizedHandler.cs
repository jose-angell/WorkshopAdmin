
using global::WorkshopAdmin.UI.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace WorkshopAdmin.UI.Auth;

public class UnauthorizedHandler : DelegatingHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly NavigationManager _navigationManager;

    public UnauthorizedHandler(IServiceProvider serviceProvider, NavigationManager navigationManager)
    {
        _serviceProvider = serviceProvider;
        _navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        // Si la API responde 401, la sesión ha caducado
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Obtenemos el ILoginService a través del ServiceProvider para evitar dependencia circular
            var loginService = _serviceProvider.GetRequiredService<ILoginService>();

            // Ejecutamos tu lógica de limpieza (remueve token del localStorage y notifica estado)
            await loginService.Logout();

            // Redirigimos al Login
            _navigationManager.NavigateTo("/login");
        }

        return response;
    }
}
