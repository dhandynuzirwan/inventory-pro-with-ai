using InventorySystem.Models;
using InventorySystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _repository.GetAllWithProductsAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdWithProductsAsync(id);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        category.CreatedAt = DateTime.Now;
        await _repository.AddAsync(category);
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        await _repository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category != null)
        {
            await _repository.DeleteAsync(category);
        }
    }
}
