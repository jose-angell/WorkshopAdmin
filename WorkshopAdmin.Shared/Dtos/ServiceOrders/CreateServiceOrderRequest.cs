namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class CreateServiceOrderRequest
{
    public Guid CustomerId { get; set; } // Obligatorio
    public Guid EquipmentId { get; set; } // Obligatorio
    public string FailureDescription { get; set; } = string.Empty;

    // Nota: La fecha de creación es automática en DB (timestamptz)
    // Se puede incluir una fecha sugerida para el inicio del diagnóstico
    public DateTimeOffset? ScheduledDate { get; set; }
}