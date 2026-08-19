using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models;

public class StockTransaction
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Jumlah harus minimal 1")]
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public Product? Product { get; set; }
}
