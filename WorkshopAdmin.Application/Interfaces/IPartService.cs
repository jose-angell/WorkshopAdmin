using WorkshopAdmin.Shared.Dtos.Parts;

namespace WorkshopAdmin.Application.Interfaces;

public interface IPartService
{
    Task<PartDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PartDto>> GetAllAsync();
    Task<IEnumerable<PartDto>> GetByNameAsync(string name);
    Task<PartDto> CreateAsync(CreatePartRequest request);
    Task UpdateAsync(UpdatePartRequest request);
    Task DeleteAsync(Guid id);
}