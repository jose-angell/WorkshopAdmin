using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Shared.Dtos.Auth;
using WorkshopAdmin.Shared.Dtos.Users;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Application.Interfaces
{
    public interface IUserService
    {
        Task<LoginDto> LoginAsync(LoginRequest login);
        Task<UserDto> AddUserAsync(CreateUserRequest user);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(UserRole? role);
        Task UpdateUserAsync(UpdateUserRequest user);
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetTechnicianById(Guid technicianId);
    }
}
