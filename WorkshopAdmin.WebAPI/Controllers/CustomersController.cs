using Microsoft.AspNetCore.Mvc;
using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Shared.Dtos.Customers;

namespace WorkshopAdmin.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Obtiene el listado completo de clientes (US 3).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Obtiene un cliente específico por su ID (PK: uuid).
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    /// <summary>
    /// Registra un nuevo cliente en el sistema (US 1).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var createdCustomer = await _customerService.CreateAsync(request);

        // Retorna 201 Created con la ruta del nuevo recurso
        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
    }

    /// <summary>
    /// Actualiza la información de un cliente existente (US 2).
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerRequest request)
    {
        await _customerService.UpdateAsync(request);
        return NoContent(); // 204 NoContent según estándares REST
    }

    /// <summary>
    /// Elimina un cliente del sistema.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _customerService.DeleteAsync(id);
        return NoContent();
    }
}