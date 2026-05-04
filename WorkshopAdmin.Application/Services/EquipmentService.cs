using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Equipments;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Application.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _repository;

    public EquipmentService(IEquipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<EquipmentDto?> GetByIdAsync(Guid id)
    {
        var equipment = await _repository.GetByIdAsync(id);
        return equipment == null ? null : MapToDto(equipment);
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllAsync()
    {
        var equipments = await _repository.GetAllAsync();
        return equipments.Select(MapToDto);
    }

    public async Task<EquipmentDto> CreateAsync(CreateEquipmentRequest request)
    {
        // Lógica de Mapeo: CreateEquipmentRequest -> Entidad Equipment (Domain)
        var equipment = new Equipment
        {
            Id = Guid.NewGuid(), // PK: uuid 
            EquipmentTypeId = request.TypeId,
            DescriptionType = request.DescriptionType,
            Brand = request.Brand,
            Model = request.Model,
            TechnicalSpecifications = request.TechnicalSpecifications,
            IsActive = true, // Nuevo equipo se asume activo por defecto
            CreatedAt = DateTimeOffset.UtcNow // timestamptz automático
        };

        await _repository.AddAsync(equipment);

        // Retorna el DTO resultante tras la persistencia
        return MapToDto(equipment);
    }

    public async Task UpdateAsync(UpdateEquipmentRequest request)
    {
        var existingEquipment = await _repository.GetByIdAsync(request.Id);
        if (existingEquipment == null) throw new NotFoundException($"Equipo con ID {request.Id} no encontrado.");
        
        existingEquipment.EquipmentTypeId = request.TypeId;
        existingEquipment.DescriptionType = request.DescriptionType;
        existingEquipment.Brand = request.Brand;
        existingEquipment.Model = request.Model;
        existingEquipment.TechnicalSpecifications = request.TechnicalSpecifications;
        existingEquipment.IsActive = request.IsActive;

        await _repository.UpdateAsync(existingEquipment);
        
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingEquipment = await _repository.GetByIdAsync(id);
        if (existingEquipment == null) throw new NotFoundException($"Equipo con ID {id} no encontrado.");

        await _repository.DeleteAsync(id);
    }

    // Helper privado de mapeo para cumplir con el contrato de Shared 
    private static EquipmentDto MapToDto(Equipment equipment) =>
        new EquipmentDto
        {
            Id = equipment.Id,
            FriendlyId = equipment.FriendlyId,
            TypeId = equipment.EquipmentTypeId,
            TypeName = equipment.EquipmentTypeId.ToFriendlyName(), 
            DescriptionType = equipment.DescriptionType,
            Brand = equipment.Brand,
            Model = equipment.Model,
            TechnicalSpecifications = equipment.TechnicalSpecifications,
            CustomerId = equipment.CustomerId,
            CustomerName = equipment.Customer.Name,
            IsActive = equipment.IsActive,
            CreatedAt = equipment.CreatedAt
        };
}