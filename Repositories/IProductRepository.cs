using InventorySystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithCategoryAsync();
    Task<Product?> GetByIdWithCategoryAsync(int id);
    Task<List<Product>> GetLowStockProductsAsync();
    Task<int> GetTotalProductsAsync();
    Task<int> GetLowStockCountAsync();
    Task<decimal> GetTotalInventoryValueAsync();
}
