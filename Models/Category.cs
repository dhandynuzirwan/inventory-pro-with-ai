using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama kategori wajib diisi")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
