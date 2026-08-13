using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama barang wajib diisi")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "SKU wajib diisi")]
    [StringLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori wajib dipilih")]
    public int CategoryId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "Harga harus lebih dari 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(0, int.MaxValue)]
    public int MinimumStock { get; set; } = 10;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Category? Category { get; set; }

    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
