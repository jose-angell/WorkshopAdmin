using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(UserRole? role);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetTechnicianById(Guid technicianId);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);

}

