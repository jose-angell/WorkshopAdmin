using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Shared.Dtos.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string RoleName => Role.ToFriendlyName();
        public bool IsActive { get; set; } = true;
        public string? Phone { get; set; }
        
        public Guid CreatedByUserId { get; set; }
        public Guid? UpdatedByUserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; } // timestamptz
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
