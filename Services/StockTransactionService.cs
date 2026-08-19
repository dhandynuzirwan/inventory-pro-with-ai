using InventorySystem.Models;
using InventorySystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services;

public class StockTransactionService
{
    private readonly IStockTransactionRepository _transactionRepo;
    private readonly IProductRepository _productRepo;

    public StockTransactionService(IStockTransactionRepository transactionRepo, IProductRepository productRepo)
    {
        _transactionRepo = transactionRepo;
        _productRepo = productRepo;
    }

    public async Task<List<StockTransaction>> GetAllAsync()
    {
        return await _transactionRepo.GetAllWithDetailsAsync();
    }

    public async Task<List<StockTransaction>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _transactionRepo.GetByDateRangeAsync(start, end);
    }

    public async Task<StockTransaction> CreateAsync(StockTransaction transaction)
    {
        var product = await _productRepo.GetByIdAsync(transaction.ProductId);
        if (product == null)
            throw new InvalidOperationException("Barang tidak ditemukan");

        // We don't check or update stock here anymore. It's done on Approval.
        transaction.Status = TransactionStatus.Pending;
        transaction.TransactionDate = DateTime.Now;
        
        await _transactionRepo.AddAsync(transaction);
        return transaction;
    }

    public async Task ApproveAsync(int id)
    {
        var transaction = await _transactionRepo.GetByIdWithProductAsync(id);
        if (transaction == null)
            throw new InvalidOperationException("Transaksi tidak ditemukan");

        if (transaction.Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Hanya transaksi Pending yang dapat disetujui");

        if (transaction.Product == null)
            throw new InvalidOperationException("Barang tidak ditemukan");

        if (transaction.Type == TransactionType.Out && transaction.Product.Stock < transaction.Quantity)
            throw new InvalidOperationException($"Stok tidak mencukupi. Stok saat ini: {transaction.Product.Stock}");

        // Update stock
        if (transaction.Type == TransactionType.In)
            transaction.Product.Stock += transaction.Quantity;
        else
            transaction.Product.Stock -= transaction.Quantity;

        transaction.Status = TransactionStatus.Approved;
        
        // Update both the transaction and the product stock
        await _transactionRepo.UpdateAsync(transaction);
    }

    public async Task RejectAsync(int id)
    {
        var transaction = await _transactionRepo.GetByIdAsync(id);
        if (transaction == null)
            throw new InvalidOperationException("Transaksi tidak ditemukan");

        if (transaction.Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Hanya transaksi Pending yang dapat ditolak");

        transaction.Status = TransactionStatus.Rejected;
        await _transactionRepo.UpdateAsync(transaction);
    }
}
