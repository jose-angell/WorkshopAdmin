using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Shared.Dtos.Users;
using WorkshopAdmin.Shared.Emuns;

namespace WorkshopAdmin.WebAPI.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost]
    public async Task<IActionResult> AddUserAsync([FromBody] CreateUserRequest user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _userService.AddUserAsync(user);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserRole? role)
    {
        var users = await _userService.GetAllUsersAsync(role);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (DomainException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/technician")]
    public async Task<IActionResult> GetTechnicianByIdAsync(Guid id)
    {
        try
        {
            var user = await _userService.GetTechnicianById(id);
            return Ok(user);
        }
        catch (DomainException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _userService.UpdateUserAsync(request);
            return NoContent(); 
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    } 
}