using WorkshopAdmin.Shared.Dtos.Auth;

namespace WorkshopAdmin.Application.Interfaces
{
    public interface IUserService
    {
        Task<LoginDto> LoginAsync(LoginRequest login);
        Task<UserDto> AddUserAsync(SignInDto user);
    }
}
