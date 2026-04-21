namespace WorkshopAdmin.Domain.Exceptions;

// Excepción específica para cuando algo no existe
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}