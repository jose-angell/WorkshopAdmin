using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Parts;

namespace WorkshopAdmin.Application.Services;

public class PartService : IPartService
{
    private readonly IPartRepository _repository;

    public PartService(IPartRepository repository)
    {
        _repository = repository;
    }

    public async Task<PartDto?> GetByIdAsync(Guid id)
    {
        var part = await _repository.GetByIdAsync(id);
        return part == null ? null : MapToDto(part);
    }

    public async Task<IEnumerable<PartDto>> GetAllAsync()
    {
        var parts = await _repository.GetAllAsync();
        return parts.Select(MapToDto);
    }

    public async Task<IEnumerable<PartDto>> GetByNameAsync(string name)
    {
        var parts = await _repository.GetByNameAsync(name);
        return parts.Select(MapToDto);
    }

    public async Task<PartDto> CreateAsync(CreatePartRequest request)
    {
        // Lógica de Mapeo: CreatePartRequest -> Entidad Part (Domain)
        var part = new Part
        {
            Id = Guid.NewGuid(), // PK: uuid
            Name = request.Name,
            Price = request.Price, // numeric(12,2)
            Stock = request.Stock, // Regla: stock >= 0
            CreatedAt = DateTimeOffset.UtcNow // timestamptz
        };

        await _repository.AddAsync(part);

        // Retorna el PartDto resultante
        return MapToDto(part);
    }

    public async Task UpdateAsync(UpdatePartRequest request)
    {
        var existingPart = await _repository.GetByIdAsync(request.Id);
        if (existingPart != null)
        {
            existingPart.Name = request.Name;
            existingPart.Price = request.Price;
            existingPart.Stock = request.Stock;

            await _repository.UpdateAsync(existingPart);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    // Helper privado de mapeo manual de Entidad a DTO
    private static PartDto MapToDto(Part part) =>
        new PartDto
        {
            Id = part.Id,
            Name = part.Name,
            Price = part.Price,
            Stock = part.Stock,
            CreatedAt = part.CreatedAt
        };
}