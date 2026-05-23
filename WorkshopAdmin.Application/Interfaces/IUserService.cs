using WorkshopAdmin.Shared.Dtos.Auth;
using WorkshopAdmin.Shared.Dtos.Users;

namespace WorkshopAdmin.Application.Interfaces
{
    public interface IUserService
    {
        Task<LoginDto> LoginAsync(LoginRequest login);
        Task<UserDto> AddUserAsync(CreateUserRequest user);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task UpdateUserAsync(UpdateUserRequest user);
        Task<UserDto?> GetUserByIdAsync(Guid id);
    }
}
