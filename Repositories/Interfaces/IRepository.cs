using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Repositories;

/// <summary>
/// Generic repository interface providing standard CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
