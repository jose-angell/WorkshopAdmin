namespace WorkshopAdmin.Shared.Emuns;

public enum UserRole
{
    Admin = 0,
    Technician = 1
}


public static class UserRoleExtensions
{
    public static string ToFriendlyName(this UserRole role) => role switch
    {
        UserRole.Admin => "Administrador",
        UserRole.Technician => "Técnico",
        _ => role.ToString()
    };
}
