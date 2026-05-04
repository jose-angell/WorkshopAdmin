using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence.Configurations;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("service_order");

        builder.HasKey(so => so.Id);

        builder.Property(so => so.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(so => so.CustomerId).HasColumnName("customer_id").HasColumnType("uuid").IsRequired();
        builder.Property(so => so.EquipmentId).HasColumnName("equipment_id").HasColumnType("uuid").IsRequired();

        builder.Property(so => so.FailureDescription)
            .HasColumnName("failure_description")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(so => so.Diagnosis)
           .HasColumnName("diagnosis")
           .HasColumnType("text");

        builder.Property(so => so.Status)
            .HasColumnName("status")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(so => so.LaborCost)
            .HasColumnName("labor_cost")
            .HasColumnType("numeric(12,2)")
            .HasDefaultValue(0);

        builder.Property(so => so.ServiceTypeId)
            .HasColumnName("service_type_id")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(so => so.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        builder.Property(so => so.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        // 1. Configurar el número secuencial
        builder.Property(so => so.OrderNumber)
            .HasColumnName("order_number")
            .HasDefaultValueSql("nextval('\"service_order_seq\"')");

        // 2. Configurar el FriendlyId como columna calculada persistente
        // Formato: ORD-YYMM-00001
        builder.Property(so => so.FriendlyId)
            .HasColumnName("friendly_id")
            // La fórmula de Postgres para generar el código al vuelo
            .HasComputedColumnSql(" 'ORD-' || lpad(order_number::text, 5, '0') ", stored: true);

        // 3. Índice único para búsquedas rápidas por el código amigable
        builder.HasIndex(so => so.FriendlyId).IsUnique();

        // Relaciones
        builder.HasOne(so => so.Customer)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CustomerId);

        builder.HasOne(so => so.Equipment)
            .WithMany(e => e.ServiceOrders)
            .HasForeignKey(so => so.EquipmentId);

        // Índices
        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.Status);
    }
}