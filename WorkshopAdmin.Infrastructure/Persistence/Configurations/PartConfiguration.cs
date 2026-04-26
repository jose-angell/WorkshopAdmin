using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence.Configurations;

public class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.ToTable("part");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id").HasColumnType("uuid");

        builder.Property(p => p.Sku)
            .HasColumnName("sku")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(p => p.Brand)
            .HasColumnName("brand")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(p => p.PartCategoryId)
            .HasColumnName("part_category_id")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(p => p.Stock)
            .HasColumnName("stock")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(p => p.MinStock)
            .HasColumnName("min_stock")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(p => p.UnitOfMeasure)
            .HasColumnName("unit_of_measure")
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder.Property(p => p.WarehouseLocation)
            .HasColumnName("warehouse_location")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(c => c.IsActive)
           .HasColumnName("is_active")
           .HasColumnType("boolean")
           .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        builder.HasIndex(x => x.Name);
    }
}