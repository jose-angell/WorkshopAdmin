using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Auth;
using WorkshopAdmin.Shared.Dtos.Users;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _PasswordHasher;
    private readonly IJwtService _jwtService;
    private readonly ICurrentUserService _currentUserService;
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _PasswordHasher = passwordHasher;
        _jwtService = jwtService;
        _currentUserService = currentUserService;
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
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(UserRole? role)
    {
        var id = _currentUserService.UserId;
        var users = await _userRepository.GetAllAsync(role);
        return users.Select(MapToDto);
    }
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new DomainException($"User with ID {id} not found.");
        }
        return MapToDto(user);
    }
    public async Task<UserDto?> GetTechnicianById(Guid technicianId)
    {
        var user = await _userRepository.GetTechnicianById(technicianId);
        if (user == null)
        {
            throw new DomainException($"Technician with ID {technicianId} not found.");
        }
        return MapToDto(user);
    }
    public async Task<UserDto> AddUserAsync(CreateUserRequest userDto)
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
            PasswordHash = _PasswordHasher.Hash(userDto.Password),
            Phone = userDto.Phone,
            CreatedByUserId = _currentUserService.UserId,
        };
        await _userRepository.AddUserAsync(newUser);
        return MapToDto(newUser);
    }

    public async Task UpdateUserAsync(UpdateUserRequest user)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
        if (existingUser == null)
        {
            throw new DomainException($"User with ID {user.Id} not found.");
        }
        existingUser.FullName = user.FullName ?? existingUser.FullName;
        existingUser.Email = user.Email ?? existingUser.Email;
        existingUser.Role = user.Role ;
        existingUser.Phone = user.Phone ?? existingUser.Phone;
        existingUser.PasswordHash = user.Password != null ? _PasswordHasher.Hash(user.Password) : existingUser.PasswordHash;

        await _userRepository.UpdateUserAsync(existingUser);
    }
   
    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            Phone = user.Phone,
            IsActive = user.IsActive,
            CreatedByUserId = user.CreatedByUserId,
            UpdatedByUserId = user.UpdatedByUserId,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}

