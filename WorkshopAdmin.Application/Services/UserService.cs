using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Auth;

namespace WorkshopAdmin.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _PasswordHasher;
    private readonly IJwtService _jwtService;
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _PasswordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginDto> LoginAsync(LoginRequest login)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(login.Email);
        if (existingUser == null) {
            return new LoginDto { Message = "Invalid email or password.", IsLoggedIn = false };
        }

        if (!_PasswordHasher.Verify(login.Password, existingUser.PasswordHash)) {
            return new LoginDto { Message = "Invalid email or password.", IsLoggedIn = false };
        }

        string token = _jwtService.GenerateToken(existingUser);

        return new LoginDto { Message = "Login successful.", IsLoggedIn = true, Token = token };

    }
    public async Task<UserDto> AddUserAsync(SignInDto userDto)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new DomainException("A user with this email already exists.");
        }
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = userDto.Email,
            FullName = userDto.FullName,
            Role = userDto.Role,
            PasswordHash = _PasswordHasher.Hash(userDto.Password)
        };
        await _userRepository.AddUserAsync(newUser);
        return MapToDto(newUser);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };
    }
}

