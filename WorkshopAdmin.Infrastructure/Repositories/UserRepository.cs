using Microsoft.EntityFrameworkCore;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Infrastructure.Persistence;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync(UserRole? role)
    {
        return await _context.Users
            .Where(u => !role.HasValue || u.Role == role.Value)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetTechnicianById(Guid technicianId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == technicianId && u.Role == UserRole.Technician);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}

