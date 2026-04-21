namespace WorkshopAdmin.Domain.Exceptions;

// Excepción base para reglas de negocio
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}