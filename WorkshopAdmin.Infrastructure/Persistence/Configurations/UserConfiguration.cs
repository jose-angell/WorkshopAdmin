using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence.Configurations;



public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(c => c.FullName)
            .HasColumnName("full_name")
            .HasColumnType("varchar(300)")
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(c => c.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("varchar(2000)")
            .IsRequired();

        builder.Property(so => so.Role)
            .HasColumnName("role")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(c => c.CreatedByUserId)
            .HasColumnName("created_by_user_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
           .HasColumnName("updated_at")
           .HasColumnType("timestamptz");

        builder.Property(c => c.UpdatedByUserId)
            .HasColumnName("updated_by_user_id")
            .HasColumnType("uuid");

        builder.Property(c => c.IsActive)
           .HasColumnName("is_active")
           .HasColumnType("boolean")
           .HasDefaultValue(true);
    }
}