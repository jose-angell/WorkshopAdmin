using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
}

