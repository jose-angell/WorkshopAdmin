using WorkshopAdmin.Application.Interfaces;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Exceptions;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Shared.Dtos.Parts;
using WorkshopAdmin.Shared.Enums;

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
        // 1. Validaciones de Reglas de Negocio (Reglas de Integridad 3 y 4) [2]
        if (request.Price < 0)
            throw new DomainException("El precio de la refacción debe ser mayor o igual a cero.");


        if (request.Stock < 0)
            throw new DomainException("El inventario inicial no puede ser negativo.");

        // 2. Lógica de Mapeo: CreatePartRequest -> Entidad Part (Domain)
        var part = new Part
        {
            Id = Guid.NewGuid(), // PK: uuid
            Sku = request.Sku, // Código único / fabricante para búsquedas 
            Name = request.Name,
            Brand = request.Brand,
            PartCategoryId = request.CategoryId,
            Price = request.Price, // tipo numeric(12,2)
            Stock = request.Stock, // Existencia actual 
            MinStock = request.MinStock, // Punto de reorden para alertas visuales
            UnitOfMeasure = request.UnitOfMeasure ?? "Pz", // Unidad (Pz, L, Kg)
            WarehouseLocation = request.WarehouseLocation, // Ubicación física 
            IsActive = true, // Estado lógico inicial 
            CreatedAt = DateTimeOffset.UtcNow // timestamptz automática 
        };

        // 3. Persistencia mediante el repositorio
        await _repository.AddAsync(part);

        // 4. Retorno del DTO (Shared) con la información procesada [7]
        return MapToDto(part);
    }

    public async Task UpdateAsync(UpdatePartRequest request)
    {
        var existingPart = await _repository.GetByIdAsync(request.Id);
        if (existingPart == null) throw new NotFoundException($"Parte con ID {request.Id} no encontrada.");

        if (request.Price < 0) throw new DomainException("El precio de la refacción debe ser mayor o igual a cero.");
        if (request.Stock < 0) throw new DomainException("El inventario no puede ser negativo.");

        existingPart.Sku = request.Sku;
        existingPart.Name = request.Name;
        existingPart.Brand = request.Brand;
        existingPart.PartCategoryId = request.CategoryId;
        existingPart.Price = request.Price;
        existingPart.Stock = request.Stock;
        existingPart.MinStock = request.MinStock;
        existingPart.UnitOfMeasure = request.UnitOfMeasure;
        existingPart.WarehouseLocation = request.WarehouseLocation;
        existingPart.IsActive = request.IsActive;

        await _repository.UpdateAsync(existingPart);
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
            Sku = part.Sku, // Código único del fabricante 
            Name = part.Name,
            Brand = part.Brand,
            // Mapeo de Categoría [1]
            CategoryId = part.PartCategoryId,
            // Uso de la extensión para obtener el nombre técnico descriptivo 
            CategoryName = part.PartCategoryId.ToFriendlyName(),
            Price = part.Price,
            Stock = part.Stock,
            MinStock = part.MinStock, // Punto de reorden para alertas visuales [
            UnitOfMeasure = part.UnitOfMeasure,
            WarehouseLocation = part.WarehouseLocation ?? string.Empty,
            IsActive = part.IsActive,
            CreatedAt = part.CreatedAt
        };
}