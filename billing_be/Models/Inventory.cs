using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Models;

public class Inventory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MedicineId { get; set; }

    [ForeignKey(nameof(MedicineId))]
    public Medicine Medicine { get; set; } = null!;

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    [Precision(10, 2)]
    public decimal Mrp { get; set; }

    [Precision(10, 2)]
    public decimal PurchasePrice { get; set; }

    [Precision(10, 2)]
    public decimal SalePrice { get; set; }

    public int StockQuantity { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    [Precision(5, 2)]
    public decimal GstPercentage { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
