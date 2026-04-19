using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Shared.Dtos.ServiceOrders;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderService _serviceOrderService;

    public ServiceOrdersController(IServiceOrderService serviceOrderService)
    {
        _serviceOrderService = serviceOrderService;
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
    /// Crea una nueva orden de servicio con estado inicial 'Received' (US 6).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceOrderRequest request)
    {
        // Se valida integridad de cliente y equipo en la capa de Application [5].
        var createdOrder = await _serviceOrderService.CreateAsync(request);

        // Retorna 201 Created con el nombre del método GetById para localizar el recurso
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
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

    /// <summary>
    /// Actualiza información general de la orden (descripción, costos de mano de obra).
    /// No permite modificar órdenes en estado 'Delivered' [7].
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateServiceOrderRequest request)
    {
        await _serviceOrderService.UpdateAsync(request);
        return NoContent();
    }
}