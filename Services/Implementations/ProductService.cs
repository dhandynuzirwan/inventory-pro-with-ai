using InventorySystem.Models;
using InventorySystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _repository.GetAllWithCategoryAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdWithCategoryAsync(id);
    }

    public async Task<List<Product>> GetLowStockProductsAsync()
    {
        return await _repository.GetLowStockProductsAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.Now;
        await _repository.AddAsync(product);
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        await _repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product != null)
        {
            await _repository.DeleteAsync(product);
        }
    }
}
