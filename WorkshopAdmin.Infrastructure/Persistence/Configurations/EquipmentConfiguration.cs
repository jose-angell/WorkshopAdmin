using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("equipment");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid");

        builder.Property(p => p.EquipmentTypeId)
            .HasColumnName("equipment_type_id")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(e => e.DescriptionType)
            .HasColumnName("description_type")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(e => e.Brand)
            .HasColumnName("brand")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(e => e.Model)
            .HasColumnName("model")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(e => e.TechnicalSpecifications)
            .HasColumnName("technical_specifications")
            .HasColumnType("varchar(2000)");

        builder.Property(c => c.IsActive)
           .HasColumnName("is_active")
           .HasColumnType("boolean")
           .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        builder.HasIndex(x => new { x.Brand, x.Model });
    }
}