using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services;

public class StockTransactionService
{
    private readonly InventoryDbContext _context;

    public StockTransactionService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockTransaction>> GetAllAsync()
    {
        return await _context.StockTransactions
            .Include(t => t.Product)
                .ThenInclude(p => p!.Category)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<List<StockTransaction>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.StockTransactions
            .Include(t => t.Product)
                .ThenInclude(p => p!.Category)
            .Where(t => t.TransactionDate >= start && t.TransactionDate <= end)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<StockTransaction> CreateAsync(StockTransaction transaction)
    {
        var product = await _context.Products.FindAsync(transaction.ProductId);
        if (product == null)
            throw new InvalidOperationException("Barang tidak ditemukan");

        if (transaction.Type == TransactionType.Out && product.Stock < transaction.Quantity)
            throw new InvalidOperationException($"Stok tidak mencukupi. Stok saat ini: {product.Stock}");

        // Update stock
        if (transaction.Type == TransactionType.In)
            product.Stock += transaction.Quantity;
        else
            product.Stock -= transaction.Quantity;

        transaction.TransactionDate = DateTime.Now;
        _context.StockTransactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }
}
