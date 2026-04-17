using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Customers;

namespace WorkshopAdmin.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request)
    {
        // Lógica de Mapeo: CreateCustomerRequest -> Entidad Customer (Domain)
        var customer = new Customer
        {
            Id = Guid.NewGuid(), // PK: uuid [4]
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAt = DateTimeOffset.UtcNow // timestamptz [4]
        };

        await _repository.AddAsync(customer);

        // Retorna el CustomerDto resultante
        return MapToDto(customer);
    }

    public async Task UpdateAsync(UpdateCustomerRequest request)
    {
        var existingCustomer = await _repository.GetByIdAsync(request.Id);
        if (existingCustomer != null)
        {
            existingCustomer.Name = request.Name;
            existingCustomer.Email = request.Email;
            existingCustomer.Phone = request.Phone;

            await _repository.UpdateAsync(existingCustomer);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    // Helper privado para mapeo de Entidad a DTO
    private static CustomerDto MapToDto(Customer customer) =>
        new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CreatedAt = customer.CreatedAt
        };
}