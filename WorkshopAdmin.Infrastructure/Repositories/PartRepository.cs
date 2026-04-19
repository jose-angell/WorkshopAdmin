using Microsoft.EntityFrameworkCore;
using WorkshopAdmin.Domain.Entities;
using WorkshopAdmin.Domain.Interfaces;
using WorkshopAdmin.Infrastructure.Persistence;

namespace WorkshopAdmin.Infrastructure.Repositories;

public class PartRepository : IPartRepository
{
    private readonly AppDbContext _context;

    public PartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Part?> GetByIdAsync(Guid id)
    {
        return await _context.Parts
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Part>> GetAllAsync()
    {
        return await _context.Parts
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Part>> GetByNameAsync(string name)
    {
        // Se utiliza Contains para facilitar la búsqueda en el inventario (US 13)
        return await _context.Parts
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
    }

    public async Task AddAsync(Part part)
    {
        await _context.Parts.AddAsync(part);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Part part)
    {
        _context.Parts.Update(part);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var part = await GetByIdAsync(id);
        if (part != null)
        {
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
        }
    }
}