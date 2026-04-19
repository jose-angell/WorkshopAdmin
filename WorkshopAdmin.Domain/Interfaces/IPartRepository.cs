using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAdmin.Domain.Entities;

namespace WorkshopAdmin.Domain.Interfaces;

public interface IPartRepository
{
    Task<Part?> GetByIdAsync(Guid id);
    Task<IEnumerable<Part>> GetAllAsync();
    Task AddAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(Guid id);

    // Método específico: Aprovecha el índice definido en part(name) [7]
    Task<IEnumerable<Part>> GetByNameAsync(string name);
}