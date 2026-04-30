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
    /// <summary>
    /// Actualiza el campo de diagnostico de la order de sevicio.
    /// No permite modificar órdenes en estado distinto a 'Diagnosting' [7].
    /// </summary>
    [HttpPatch("{id:guid}/diagnosis")]
    public async Task<IActionResult> UpdateDiagnosis(Guid id, [FromBody] string diagnosis)
    {
        await _serviceOrderService.UpdateDiagnosisAsync(id, diagnosis);
        return NoContent();
    }
    /// <summary>
    /// Asigna una refacción a la orden de servicio (US 15).
    /// Registra el precio histórico y descuenta el stock (Regla 5.3).
    /// </summary>
    [HttpPost("parts")]
    public async Task<IActionResult> AddPartToOrder([FromBody] CreateOrderPartRequest request)
    {
        // La lógica de aplicación valida stock suficiente y estado de la orden (Regla 5.6)
        await _serviceOrderService.AddPartToOrderAsync(request);

        // Retornamos 200 Ok ya que es una operación exitosa sobre una relación existente
        return Ok("Refacción asignada correctamente a la orden.");
    }
    /// <summary>
    /// Actualiza la cantidad de una refacción ya asignada.
    /// Ajusta automáticamente la diferencia en el inventario (US 16).
    /// </summary>
    [HttpPut("parts")]
    public async Task<IActionResult> UpdatePartToOrder([FromBody] UpdateOrderPartRequest request)
    {
        // No se permite modificar refacciones si la orden está en estado 'Completed' o 'Delivered'
        await _serviceOrderService.UpdatePartToOrderAsync(request);

        return NoContent();
    }
}