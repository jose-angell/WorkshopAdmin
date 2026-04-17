using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);

    // Método específico para búsqueda por nombre (Requisito US 1.3)
    Task<IEnumerable<Customer>> SearchByNameAsync(string name);
}