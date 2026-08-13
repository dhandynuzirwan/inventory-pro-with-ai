using Microsoft.EntityFrameworkCore;
using InventorySystem.Models;

namespace InventorySystem.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Elektronik", Description = "Perangkat elektronik dan gadget", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 2, Name = "Alat Tulis", Description = "Perlengkapan alat tulis kantor", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 3, Name = "Furnitur", Description = "Perabot kantor dan rumah tangga", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 4, Name = "Makanan & Minuman", Description = "Produk konsumsi makanan dan minuman", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 5, Name = "Pakaian", Description = "Pakaian dan aksesoris", CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop ASUS VivoBook", SKU = "ELK-001", CategoryId = 1, Price = 8500000m, Stock = 25, MinimumStock = 5, Description = "Laptop 14 inch, RAM 8GB, SSD 512GB", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 2, Name = "Mouse Wireless Logitech", SKU = "ELK-002", CategoryId = 1, Price = 350000m, Stock = 50, MinimumStock = 15, Description = "Mouse wireless ergonomis", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 3, Name = "Keyboard Mechanical", SKU = "ELK-003", CategoryId = 1, Price = 750000m, Stock = 3, MinimumStock = 10, Description = "Keyboard mechanical RGB", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 4, Name = "Pulpen Pilot G2", SKU = "ATK-001", CategoryId = 2, Price = 15000m, Stock = 200, MinimumStock = 50, Description = "Pulpen gel 0.5mm", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 5, Name = "Buku Tulis A5", SKU = "ATK-002", CategoryId = 2, Price = 8000m, Stock = 150, MinimumStock = 30, Description = "Buku tulis 100 lembar", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 6, Name = "Kertas HVS A4", SKU = "ATK-003", CategoryId = 2, Price = 55000m, Stock = 8, MinimumStock = 20, Description = "Kertas HVS 80gsm 500 lembar", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 7, Name = "Meja Kerja Minimalis", SKU = "FRN-001", CategoryId = 3, Price = 1200000m, Stock = 12, MinimumStock = 3, Description = "Meja kerja kayu 120x60cm", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 8, Name = "Kursi Kantor Ergonomis", SKU = "FRN-002", CategoryId = 3, Price = 2500000m, Stock = 2, MinimumStock = 5, Description = "Kursi kantor dengan sandaran mesh", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 9, Name = "Air Mineral 600ml", SKU = "MKN-001", CategoryId = 4, Price = 4000m, Stock = 500, MinimumStock = 100, Description = "Air mineral kemasan botol", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 10, Name = "Kopi Sachet Kapal Api", SKU = "MKN-002", CategoryId = 4, Price = 2500m, Stock = 5, MinimumStock = 50, Description = "Kopi instan sachet 25g", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 11, Name = "Kaos Polo Hitam", SKU = "PKN-001", CategoryId = 5, Price = 125000m, Stock = 40, MinimumStock = 10, Description = "Kaos polo cotton combed 30s", CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 12, Name = "Kemeja Putih Formal", SKU = "PKN-002", CategoryId = 5, Price = 185000m, Stock = 7, MinimumStock = 10, Description = "Kemeja katun formal lengan panjang", CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Transactions
        modelBuilder.Entity<StockTransaction>().HasData(
            new StockTransaction { Id = 1, ProductId = 1, Type = TransactionType.In, Quantity = 30, Notes = "Stok awal", TransactionDate = new DateTime(2024, 1, 15) },
            new StockTransaction { Id = 2, ProductId = 1, Type = TransactionType.Out, Quantity = 5, Notes = "Penjualan", TransactionDate = new DateTime(2024, 2, 10) },
            new StockTransaction { Id = 3, ProductId = 2, Type = TransactionType.In, Quantity = 60, Notes = "Restok dari supplier", TransactionDate = new DateTime(2024, 1, 20) },
            new StockTransaction { Id = 4, ProductId = 2, Type = TransactionType.Out, Quantity = 10, Notes = "Penjualan online", TransactionDate = new DateTime(2024, 3, 5) },
            new StockTransaction { Id = 5, ProductId = 3, Type = TransactionType.In, Quantity = 15, Notes = "Stok awal", TransactionDate = new DateTime(2024, 1, 10) },
            new StockTransaction { Id = 6, ProductId = 3, Type = TransactionType.Out, Quantity = 12, Notes = "Penjualan grosir", TransactionDate = new DateTime(2024, 3, 15) },
            new StockTransaction { Id = 7, ProductId = 4, Type = TransactionType.In, Quantity = 250, Notes = "Pembelian bulk", TransactionDate = new DateTime(2024, 2, 1) },
            new StockTransaction { Id = 8, ProductId = 4, Type = TransactionType.Out, Quantity = 50, Notes = "Distribusi ke cabang", TransactionDate = new DateTime(2024, 3, 20) },
            new StockTransaction { Id = 9, ProductId = 8, Type = TransactionType.In, Quantity = 10, Notes = "Pengadaan kantor baru", TransactionDate = new DateTime(2024, 2, 15) },
            new StockTransaction { Id = 10, ProductId = 8, Type = TransactionType.Out, Quantity = 8, Notes = "Distribusi ke kantor cabang", TransactionDate = new DateTime(2024, 3, 1) },
            new StockTransaction { Id = 11, ProductId = 10, Type = TransactionType.In, Quantity = 100, Notes = "Stok awal", TransactionDate = new DateTime(2024, 1, 5) },
            new StockTransaction { Id = 12, ProductId = 10, Type = TransactionType.Out, Quantity = 95, Notes = "Pemakaian harian", TransactionDate = new DateTime(2024, 3, 25) }
        );
    }
}
