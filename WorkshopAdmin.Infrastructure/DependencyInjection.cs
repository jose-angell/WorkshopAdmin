using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Infrastructure.Persistence;
using WorkshopAdmin.Infrastructure.Repositories;

namespace WorkshopAdmin.Infrastructure;

/// <summary>
/// Clase estática para la configuración de la inyección de dependencias de la capa de infraestructura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de infraestructura, incluyendo la persistencia en PostgreSQL.
    /// </summary>
    /// <param name="services">Colección de servicios de IServiceCollection.</param>
    /// <param name="configuration">Configuración de la aplicación para leer la cadena de conexión.</param>
    /// <returns>La colección de servicios para encadenamiento.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Configuración de AppDbContext con PostgreSQL
        // Se utiliza la convención de nombres del Data Dictionary (PostgreSQL) [1, 3]
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // 2. Registro de Repositorios (Aquí se registrarán las implementaciones más adelante)
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();

        return services;
    }
}