using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customer");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(c => c.Phone)
            .HasColumnName("phone")
            .HasColumnType("varchar(20)");

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(150)");

        builder.Property(c => c.IsActive)
           .HasColumnName("is_active")
           .HasColumnType("boolean")
           .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(c => c.CustomerNumber)
            .HasColumnName("customer_number")
            .HasDefaultValueSql("nextval('\"customer_seq\"')");

        // Formato: CUST-00001
        builder.Property(c => c.FriendlyId)
            .HasColumnName("friendly_id")
            .HasComputedColumnSql(" 'CUST-' || lpad(customer_number::text, 5, '0') ", stored: true);
       
        builder.HasIndex(c => c.FriendlyId).IsUnique();
        builder.HasIndex(x => x.Name);
    }
}