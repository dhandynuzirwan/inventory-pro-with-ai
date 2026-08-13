using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services;

public class DashboardService
{
    private readonly InventoryDbContext _context;

    public DashboardService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<int> GetLowStockCountAsync()
    {
        return await _context.Products.CountAsync(p => p.Stock <= p.MinimumStock);
    }

    public async Task<int> GetTotalCategoriesAsync()
    {
        return await _context.Categories.CountAsync();
    }

    public async Task<decimal> GetTotalInventoryValueAsync()
    {
        return await _context.Products.SumAsync(p => p.Price * p.Stock);
    }

    public async Task<List<CategoryStockData>> GetStockByCategoryAsync()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .Select(c => new CategoryStockData
            {
                CategoryName = c.Name,
                TotalStock = c.Products.Sum(p => p.Stock),
                ProductCount = c.Products.Count
            })
            .Where(c => c.ProductCount > 0)
            .ToListAsync();
    }

    public async Task<List<TransactionTrendData>> GetTransactionTrendAsync(int days = 30)
    {
        var startDate = DateTime.Now.AddDays(-days).Date;
        var transactions = await _context.StockTransactions
            .Where(t => t.TransactionDate >= startDate)
            .ToListAsync();

        return transactions
            .GroupBy(t => t.TransactionDate.Date)
            .Select(g => new TransactionTrendData
            {
                Date = g.Key,
                InCount = g.Where(t => t.Type == TransactionType.In).Sum(t => t.Quantity),
                OutCount = g.Where(t => t.Type == TransactionType.Out).Sum(t => t.Quantity)
            })
            .OrderBy(t => t.Date)
            .ToList();
    }

    public async Task<List<StockTransaction>> GetRecentTransactionsAsync(int count = 10)
    {
        return await _context.StockTransactions
            .Include(t => t.Product)
            .OrderByDescending(t => t.TransactionDate)
            .Take(count)
            .ToListAsync();
    }
}

public class CategoryStockData
{
    public string CategoryName { get; set; } = string.Empty;
    public int TotalStock { get; set; }
    public int ProductCount { get; set; }
}

public class TransactionTrendData
{
    public DateTime Date { get; set; }
    public int InCount { get; set; }
    public int OutCount { get; set; }
}
