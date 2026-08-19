using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(InventoryDbContext context) : base(context)
    {
    }

    public async Task<List<Product>> GetAllWithCategoryAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdWithCategoryAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetLowStockProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.Stock <= p.MinimumStock)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> GetLowStockCountAsync()
    {
        return await _dbSet.CountAsync(p => p.Stock <= p.MinimumStock);
    }

    public async Task<decimal> GetTotalInventoryValueAsync()
    {
        return await _dbSet.SumAsync(p => p.Price * p.Stock);
    }
}
