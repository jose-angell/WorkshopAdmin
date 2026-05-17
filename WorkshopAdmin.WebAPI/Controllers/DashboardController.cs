using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Shared.Dtos.Dashboard;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.WebAPI.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IServiceOrderService _serviceOrderService;
    private readonly IDashboardService _dashboardService;

    public DashboardController(IServiceOrderService serviceOrderService, IDashboardService dashboardService)
    {
        _serviceOrderService = serviceOrderService;
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Obtiene el listado de órdenes con filtros opcionales por estado o cliente (US 9).
    /// </summary>
    /// <param name="status">Estado de la orden (0-4) [3].</param>
    /// <param name="customerId">Identificador del cliente (uuid) [4].</param>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ServiceOrderStatus? status, [FromQuery] Guid? customerId)
    {
        // El servicio maneja la lógica de filtrado basada en los Query Parameters
        var orders = await _serviceOrderService.GetAllFilteredAsync(status, customerId);
        return Ok(orders);
    }
    /// <summary>
    /// Obtiene las estadísticas generales del sistema (US 9).
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        // El servicio maneja la lógica de filtrado basada en los Query Parameters
        var stats = await _dashboardService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Obtiene la tendencia del volumen de servicios (US 12).
    /// </summary>
    [HttpGet("service-volume-trend")]
    public async Task<IActionResult> GetServiceVolumeTrend()
    {
        var trend = await _dashboardService.GetServiceVolumeTrendAsync();
        return Ok(trend);
    }
    /// <summary>
    /// Obtiene el listado de partes con stock bajo (US 14).
    /// </summary>
    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<LowStockPartDto>>> GetLowStockParts()
    {
        var result =
            await _dashboardService.GetLowStockPartsAsync();

        return Ok(result);
    }

    /// <summary>
    /// Obtiene el detalle completo de una orden específica (US 10).
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _serviceOrderService.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    
    /// <summary>
    /// Actualiza únicamente el estado de la orden siguiendo el flujo permitido (US 11).
    /// Endpoint: PATCH api/ServiceOrders/{id}/status
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateServiceOrderStatusRequest request)
    {
        // El ID de la ruta debe coincidir con el del cuerpo para asegurar integridad
        if (id != request.Id) return BadRequest("El ID de la ruta no coincide con el cuerpo de la solicitud.");

        // El servicio debe validar las reglas de flujo (ej. no saltar Diagnosing) [6].
        await _serviceOrderService.UpdateStatusAsync(request);

        return NoContent();
    }

    
}