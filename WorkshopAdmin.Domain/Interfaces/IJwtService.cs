using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Domain.Interfaces;

public interface IJwtService
{
    // Recibe la entidad User (Domain) y devuelve el token firmado
    string GenerateToken(User user);
}
