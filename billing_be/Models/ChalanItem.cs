namespace billing_be.Models
{
    public class ChalanItem
    {
        public int Id { get; set; }
        public int? MedicineId { get; set; } // Nullable if it's a new/adhoc entry
        public string MedicineName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal TaxRate { get; set; } // Percentage
        public decimal Discount { get; set; } // Percentage
        public decimal TotalAmount { get; set; }
        public string? Remarks { get; set; }
    }
}
