using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;

namespace WorkshopAdmin.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        // 1. Obtener la clave desde appsettings.json
        var secretKey = _configuration["Jwt:Key"]
            ?? throw new Exception("JWT Key no configurada.");
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);

        // 2. Definir los Claims (identidad del usuario) basado en tus fuentes [7-9]
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()) // Admin o Tecnico [10, 11]
        };

        // 3. Configurar la firma y expiración [6, 12]
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(1440), // 24 horas
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        // 4. Generar el token final
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}