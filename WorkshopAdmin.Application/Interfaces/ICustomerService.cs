using WorkshopAdmin.Shared.Dtos.Customers;

namespace WorkshopAdmin.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto> CreateAsync(CreateCustomerRequest request);
    Task UpdateAsync(UpdateCustomerRequest request);
    Task DeleteAsync(Guid id);
    Task IsActivateAsync(Guid id);
}