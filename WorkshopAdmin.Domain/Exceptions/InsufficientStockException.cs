using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAdmin.Domain.Exceptions;

// Excepción para el stock
public class InsufficientStockException : DomainException
{
    public InsufficientStockException() : base("No hay suficiente stock para realizar esta operación.") { }
}