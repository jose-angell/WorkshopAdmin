using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAdmin.Application.Interfaces;

public interface ICurrentUserService
{
    // Retorna el ID del usuario autenticado como Guid (UUID)
    // Se define como anulable para casos donde no hay sesión activa
    Guid UserId { get; }
}
