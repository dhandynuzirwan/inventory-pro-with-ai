using InventorySystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetLowStockProductsAsync();
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}