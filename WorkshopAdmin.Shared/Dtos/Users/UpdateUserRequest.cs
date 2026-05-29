using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Shared.Dtos.Users;
public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; } = null;
    public bool IsActive { get; set; }
}

