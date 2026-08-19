using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

public interface IStockTransactionRepository : IRepository<StockTransaction>
{
    Task<List<StockTransaction>> GetAllWithDetailsAsync();
    Task<List<StockTransaction>> GetByDateRangeAsync(DateTime start, DateTime end);
    Task<StockTransaction?> GetByIdWithProductAsync(int id);
    Task<List<StockTransaction>> GetRecentTransactionsAsync(int count);
    Task<List<StockTransaction>> GetTransactionsFromDateAsync(DateTime startDate);
}
