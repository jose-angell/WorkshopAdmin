using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Domain.Interfaces;

public interface IEquipmentRepository
{
    Task<Equipment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Equipment>> GetAllAsync();
    Task AddAsync(Equipment equipment);
    Task UpdateAsync(Equipment equipment);
    Task DeleteAsync(Guid id);

    // Método específico: Permite reutilizar información (US 5) y aprovecha índices (Brand, Model)
    Task<IEnumerable<Equipment>> GetByBrandAndModelAsync(string brand, string model);
}