using Microsoft.EntityFrameworkCore;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Infrastructure.Persistence;

namespace WorkshopAdmin.Infrastructure.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly AppDbContext _context;

    public EquipmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Equipment?> GetByIdAsync(Guid id)
    {
        return await _context.Equipments
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Equipment>> GetAllAsync()
    {
        return await _context.Equipments
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Equipment>> GetByBrandAndModelAsync(string brand, string model)
    {
        return await _context.Equipments
            .Where(e => e.Brand.Contains(brand) && e.Model.Contains(model))
            .ToListAsync();
    }

    public async Task AddAsync(Equipment equipment)
    {
        await _context.Equipments.AddAsync(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        _context.Equipments.Update(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await GetByIdAsync(id);
        if (equipment != null)
        {
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
        }
    }
}