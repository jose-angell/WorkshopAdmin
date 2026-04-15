using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence.Configurations;

public class OrderPartConfiguration : IEntityTypeConfiguration<OrderPart>
{
    public void Configure(EntityTypeBuilder<OrderPart> builder)
    {
        builder.ToTable("order_part");

        // Clave Compuesta
        builder.HasKey(op => new { op.ServiceOrderId, op.PartId });

        builder.Property(op => op.ServiceOrderId)
            .HasColumnName("service_order_id")
            .HasColumnType("uuid");

        builder.Property(op => op.PartId)
            .HasColumnName("part_id")
            .HasColumnType("uuid");

        builder.Property(op => op.Quantity)
            .HasColumnName("quantity")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(op => op.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("numeric(12,2)")
            .IsRequired(); // Representa el precio histórico

        // Relaciones
        builder.HasOne(op => op.ServiceOrder)
            .WithMany(so => so.OrderParts)
            .HasForeignKey(op => op.ServiceOrderId);

        builder.HasOne(op => op.Part)
            .WithMany(p => p.OrderParts)
            .HasForeignKey(op => op.PartId);
    }
}