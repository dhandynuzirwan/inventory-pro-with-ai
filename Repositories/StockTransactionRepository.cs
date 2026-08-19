using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

public class StockTransactionRepository : Repository<StockTransaction>, IStockTransactionRepository
{
    public StockTransactionRepository(InventoryDbContext context) : base(context)
    {
    }

    public async Task<List<StockTransaction>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(t => t.Product)
                .ThenInclude(p => p!.Category)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<List<StockTransaction>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Include(t => t.Product)
                .ThenInclude(p => p!.Category)
            .Where(t => t.TransactionDate >= start && t.TransactionDate <= end)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<StockTransaction?> GetByIdWithProductAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Product)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<StockTransaction>> GetRecentTransactionsAsync(int count)
    {
        return await _dbSet
            .Include(t => t.Product)
            .OrderByDescending(t => t.TransactionDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<StockTransaction>> GetTransactionsFromDateAsync(DateTime startDate)
    {
        return await _dbSet
            .Where(t => t.TransactionDate >= startDate)
            .ToListAsync();
    }
}
