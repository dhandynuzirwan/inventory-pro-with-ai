using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(InventoryDbContext context) : base(context)
    {
    }

    public async Task<List<Category>> GetAllWithProductsAsync()
    {
        return await _dbSet
            .Include(c => c.Products)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdWithProductsAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
