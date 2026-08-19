using InventorySystem.Models;
using InventorySystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public class DashboardService : IDashboardService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IStockTransactionRepository _transactionRepo;

    public DashboardService(IProductRepository productRepo, ICategoryRepository categoryRepo, IStockTransactionRepository transactionRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _transactionRepo = transactionRepo;
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _productRepo.GetTotalProductsAsync();
    }

    public async Task<int> GetLowStockCountAsync()
    {
        return await _productRepo.GetLowStockCountAsync();
    }

    public async Task<int> GetTotalCategoriesAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        return categories.Count();
    }

    public async Task<decimal> GetTotalInventoryValueAsync()
    {
        return await _productRepo.GetTotalInventoryValueAsync();
    }

    public async Task<List<CategoryStockData>> GetStockByCategoryAsync()
    {
        var categories = await _categoryRepo.GetAllWithProductsAsync();
        return categories
            .Select(c => new CategoryStockData
            {
                CategoryName = c.Name,
                TotalStock = c.Products.Sum(p => p.Stock),
                ProductCount = c.Products.Count
            })
            .Where(c => c.ProductCount > 0)
            .ToList();
    }

    public async Task<List<TransactionTrendData>> GetTransactionTrendAsync(int days = 30)
    {
        var startDate = DateTime.Now.AddDays(-days).Date;
        var transactions = await _transactionRepo.GetTransactionsFromDateAsync(startDate);

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
        return await _transactionRepo.GetRecentTransactionsAsync(count);
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
