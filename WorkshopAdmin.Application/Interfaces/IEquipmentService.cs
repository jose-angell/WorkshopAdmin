using WorkshopAdmin.Shared.Dtos.Equipments;

namespace WorkshopAdmin.Application.Interfaces;

public interface IEquipmentService
{
    Task<EquipmentDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<EquipmentDto>> GetAllAsync();
    Task<EquipmentDto> CreateAsync(CreateEquipmentRequest request);
    Task UpdateAsync(UpdateEquipmentRequest request);
    Task DeleteAsync(Guid id);
}
