using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Shared.Dtos.Equipments;

namespace WorkshopAdmin.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    /// <summary>
    /// Obtiene el listado de todos los equipos registrados (US 5).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var equipments = await _equipmentService.GetAllAsync();
        return Ok(equipments);
    }

    /// <summary>
    /// Obtiene un equipo específico por su ID (PK: uuid).
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var equipment = await _equipmentService.GetByIdAsync(id);

        if (equipment == null)
        {
            return NotFound();
        }

        return Ok(equipment);
    }

    /// <summary>
    /// Registra un nuevo equipo en el sistema (US 4).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEquipmentRequest request)
    {
        var createdEquipment = await _equipmentService.CreateAsync(request);

        // Retorna 201 Created con la ruta para consultar el nuevo equipo
        return CreatedAtAction(nameof(GetById), new { id = createdEquipment.Id }, createdEquipment);
    }

    /// <summary>
    /// Actualiza la información técnica de un equipo existente.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEquipmentRequest request)
    {
        await _equipmentService.UpdateAsync(request);
        return NoContent();
    }

    /// <summary>
    /// Elimina un equipo del registro.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _equipmentService.DeleteAsync(id);
        return NoContent();
    }
}