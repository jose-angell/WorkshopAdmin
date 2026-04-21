namespace WorkshopAdmin.Shared.Dtos;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; } // Solo en desarrollo
    public string TraceId { get; set; } = string.Empty;
}