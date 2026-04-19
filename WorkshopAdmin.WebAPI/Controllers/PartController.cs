using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Shared.Dtos.Parts;

namespace WorkshopAdmin.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartController : ControllerBase
{
    private readonly IPartService _partService;

    public PartController(IPartService partService)
    {
        _partService = partService;
    }

    /// <summary>
    /// Obtiene el inventario completo de refacciones (US 13).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var parts = await _partService.GetAllAsync();
        return Ok(parts);
    }

    /// <summary>
    /// Obtiene una refacción específica por su identificador único (uuid).
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var part = await _partService.GetByIdAsync(id);

        if (part == null)
        {
            return NotFound();
        }

        return Ok(part);
    }

    /// <summary>
    /// Busca refacciones en el inventario por coincidencia de nombre.
    /// Aprovecha el índice definido en la base de datos (part_name_idx).
    /// </summary>
    [HttpGet("search/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var parts = await _partService.GetByNameAsync(name);
        return Ok(parts);
    }

    /// <summary>
    /// Registra una nueva refacción en el inventario (US 13).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartRequest request)
    {
        var createdPart = await _partService.CreateAsync(request);

        // Retorna 201 Created con la ubicación del nuevo recurso
        return CreatedAtAction(nameof(GetById), new { id = createdPart.Id }, createdPart);
    }

    /// <summary>
    /// Actualiza la información (precio, stock o nombre) de una refacción existente.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePartRequest request)
    {
        await _partService.UpdateAsync(request);
        return NoContent(); // 204 según estándares para actualizaciones exitosas
    }

    /// <summary>
    /// Elimina una refacción del inventario.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _partService.DeleteAsync(id);
        return NoContent();
    }
}