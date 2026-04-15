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

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(e => e.Brand)
            .HasColumnName("brand")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(e => e.Model)
            .HasColumnName("model")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
    }
}