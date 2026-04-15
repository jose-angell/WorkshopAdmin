using Microsoft.EntityFrameworkCore;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<OrderPart> OrderParts => Set<OrderPart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas las configuraciones que implementan IEntityTypeConfiguration 
        // definidas en este ensamblado 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}