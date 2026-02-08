using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace billing_be.Models;

public class PurchaseChalan
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; } // The Supplier

    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }

    [Required, MaxLength(50)]
    public string InvoiceNumber { get; set; } = null!;

    public DateOnly InvoiceDate { get; set; }

    [MaxLength(50)]
    public string? PaymentTerms { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<PurchaseChalanItem> Items { get; set; } = new();
}

public class PurchaseChalanItem
{
    [Key]
    public int Id { get; set; }

    public int PurchaseChalanId { get; set; }

    [ForeignKey(nameof(PurchaseChalanId))]
    public PurchaseChalan? PurchaseChalan { get; set; }

    public int MedicineId { get; set; }

    [ForeignKey(nameof(MedicineId))]
    public Medicine? Medicine { get; set; }

    [MaxLength(50)]
    public string BatchNumber { get; set; } = string.Empty;

    public DateOnly? ExpiryDate { get; set; }

    public int Quantity { get; set; }
    public int FreeQuantity { get; set; }

    // Price Details
    public decimal PurchaseRate { get; set; }
    public decimal Mrp { get; set; }       // Needed for Inventory
    public decimal SalePrice { get; set; } // Needed for Inventory

    public decimal GstPercentage { get; set; }
    public decimal Amount { get; set; }
}