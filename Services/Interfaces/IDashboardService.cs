using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public interface IDashboardService
{
    Task<int> GetTotalProductsAsync();
    Task<int> GetLowStockCountAsync();
    Task<int> GetTotalCategoriesAsync();
    Task<decimal> GetTotalInventoryValueAsync();
    Task<List<CategoryStockData>> GetStockByCategoryAsync();
    Task<List<TransactionTrendData>> GetTransactionTrendAsync(int days = 30);
    Task<List<StockTransaction>> GetRecentTransactionsAsync(int count = 10);
}