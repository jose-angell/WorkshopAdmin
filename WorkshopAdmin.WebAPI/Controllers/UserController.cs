using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Shared.Dtos.Users;

namespace WorkshopAdmin.WebAPI.Controllers;

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
    public async Task<IActionResult> AddUserAsync([FromBody] UserRequest user)
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
}

