using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Domain.Entities;

public class User: BaseEntity
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public UserRole Role { get; set; }

    public bool IsActive { get; set; } = true;
}