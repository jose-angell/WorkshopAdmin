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

        builder.Property(so => so.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        builder.Property(so => so.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

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