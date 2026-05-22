using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Shared.Dtos.Auth;

namespace WorkshopAdmin.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> loginAsync([FromBody] LoginRequest login)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var rsult = await _userService.LoginAsync(login);

            return Ok(rsult);
        }

    }
}
