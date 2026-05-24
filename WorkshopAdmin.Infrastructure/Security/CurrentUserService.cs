using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WorkshopAdmin.Application.Interfaces;

namespace WorkshopAdmin.Infrastructure.Security;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(id))
            {
                throw new UnauthorizedAccessException("El identificador del usuario no está presente en el contexto actual.");
            }

            return Guid.Parse(id);
        }
    }
}