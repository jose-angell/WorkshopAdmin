using Microsoft.Extensions.DependencyInjection;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Application.Services;

namespace WorkshopAdmin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registro del servicio de aplicación
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}