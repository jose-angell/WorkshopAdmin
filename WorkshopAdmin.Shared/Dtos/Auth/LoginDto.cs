using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAdmin.Shared.Dtos.Auth;
public class LoginDto
{
    public string Message { get; set; } = string.Empty;
    public bool IsLoggedIn { get; set; }
    public string Token { get; set; } = string.Empty;
}

