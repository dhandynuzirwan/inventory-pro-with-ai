using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public interface IStockTransactionService
{
    Task<List<StockTransaction>> GetAllAsync();
    Task<List<StockTransaction>> GetByDateRangeAsync(DateTime start, DateTime end);
    Task<StockTransaction> CreateAsync(StockTransaction transaction);
    Task ApproveAsync(int id);
    Task RejectAsync(int id);
}