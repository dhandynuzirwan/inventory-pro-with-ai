using InventorySystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllWithProductsAsync();
    Task<Category?> GetByIdWithProductsAsync(int id);
}
