using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Shared.Dtos.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string RoleName => Role.ToString();
    }
}
