using System.Net;
using System.Text.Json;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Shared.Dtos;

namespace WorkshopAdmin.WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si algo falla en cualquier capa, cae aquí
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var traceId = Guid.NewGuid().ToString();

        // 1. Mapeo HTTP: Decidimos el código de estado según el tipo de error
        context.Response.StatusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,         // 404
            DomainException => (int)HttpStatusCode.BadRequest,          // 400
            _ => (int)HttpStatusCode.InternalServerError                // 500
        };

        // 2. Logging: Guardar el error con su TraceId para buscarlo después
        if (context.Response.StatusCode == 500)
        {
            _logger.LogError(exception, "Error crítico [{TraceId}]: {Message}", traceId, exception.Message);
        }
        else
        {
            _logger.LogWarning("Advertencia de negocio [{TraceId}]: {Message}", traceId, exception.Message);
        }

        // 3. Respuesta Estándar
        var response = new ErrorResponse
        {
            Message = exception.Message,
            TraceId = traceId,
            // Solo mostramos el StackTrace detallado si estamos programando en local
            Details = _env.IsDevelopment() ? exception.StackTrace : "Contacte al administrador."
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}