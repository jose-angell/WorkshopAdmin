using Microsoft.EntityFrameworkCore;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Infrastructure.Persistence;

public class DbSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DbSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        // 1. Asegurar que la base de datos existe (PostgreSQL)
        await _context.Database.MigrateAsync();

        // 2. Verificar si ya existe un administrador para evitar duplicados
        if (!await _context.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            await CreateFirstAdminAsync();
        }
    }

    private async Task CreateFirstAdminAsync()
    {
        var adminId = Guid.NewGuid();

        var adminUser = new User
        {
            Id = adminId,
            FullName = "Administrador Principal",
            Email = "admin@workshop.com",
            // Usamos el servicio de infraestructura para el hash
            PasswordHash = _passwordHasher.Hash("Admin123*"),
            Role = UserRole.Admin,
            IsActive = true,
            Phone = "555-1234",
            // Campos de BaseEntity (Auditoría)
            CreatedAt = DateTimeOffset.UtcNow,
            // Regla técnica: El primer admin se referencia a sí mismo
            CreatedByUserId = adminId
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();
    }
}