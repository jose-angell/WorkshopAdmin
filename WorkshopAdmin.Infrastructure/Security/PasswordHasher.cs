using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAdmin.Domain.Interfaces;

namespace WorkshopAdmin.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);

    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
}