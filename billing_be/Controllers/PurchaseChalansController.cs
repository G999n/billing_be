using billing_be.Data;
using billing_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseChalansController : ControllerBase
{
    private readonly AppDbContext _context;

    public PurchaseChalansController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(PurchaseChalanDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Create the Chalan Header
            var chalan = new PurchaseChalan
            {
                ClientId = dto.ClientId,
                InvoiceNumber = dto.InvoiceNumber,
                InvoiceDate = dto.InvoiceDate,
                PaymentTerms = dto.PaymentTerms,
                TotalAmount = dto.TotalAmount,
                Items = new List<PurchaseChalanItem>()
            };

            foreach (var itemDto in dto.Items)
            {
                // 2. Create Chalan Items (History)
                var item = new PurchaseChalanItem
                {
                    MedicineId = itemDto.MedicineId,
                    BatchNumber = itemDto.BatchNumber,
                    ExpiryDate = itemDto.ExpiryDate,
                    Quantity = itemDto.Quantity,
                    FreeQuantity = itemDto.FreeQuantity,
                    PurchaseRate = itemDto.PurchaseRate,
                    Mrp = itemDto.Mrp,
                    SalePrice = itemDto.SalePrice,
                    GstPercentage = itemDto.GstPercentage,
                    Amount = itemDto.Amount
                };
                chalan.Items.Add(item);

                // 3. UPDATE INVENTORY (The "Live" Stock)
                // Check if we already have this Medicine + Batch in inventory
                var existingStock = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.MedicineId == itemDto.MedicineId && i.BatchNumber == itemDto.BatchNumber);

                if (existingStock != null)
                {
                    // Update existing batch
                    existingStock.StockQuantity += (itemDto.Quantity + itemDto.FreeQuantity);
                    existingStock.PurchasePrice = itemDto.PurchaseRate; // Update latest prices
                    existingStock.Mrp = itemDto.Mrp;
                    existingStock.SalePrice = itemDto.SalePrice;
                }
                else
                {
                    // Create new batch entry
                    var newStock = new Inventory
                    {
                        MedicineId = itemDto.MedicineId,
                        BatchNumber = itemDto.BatchNumber,
                        ExpiryDate = itemDto.ExpiryDate,
                        StockQuantity = (itemDto.Quantity + itemDto.FreeQuantity),
                        PurchasePrice = itemDto.PurchaseRate,
                        Mrp = itemDto.Mrp,
                        SalePrice = itemDto.SalePrice,
                        GstPercentage = itemDto.GstPercentage,
                        IsActive = true
                    };
                    _context.Inventories.Add(newStock);
                }
            }

            _context.PurchaseChalans.Add(chalan);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return Ok(new { Message = "Purchase saved and stock updated successfully!", ChalanId = chalan.Id });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "Error creating purchase: " + ex.Message);
        }
    }

    // GET: api/PurchaseChalans
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseChalan>>> GetAll()
    {
        return await _context.PurchaseChalans
            .Include(p => p.Client) // Load Supplier Name
            .Include(p => p.Items)  // Load Items (for count/details)
            .ThenInclude(i => i.Medicine) // Load Medicine Names
            .OrderByDescending(p => p.CreatedAt) // Newest first
            .ToListAsync();
    }
}

// DTOs to handle the incoming JSON structure
public class PurchaseChalanDto
{
    public int ClientId { get; set; }
    public string InvoiceNumber { get; set; } = "";
    public DateOnly InvoiceDate { get; set; }
    public string? PaymentTerms { get; set; }
    public decimal TotalAmount { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new();
}

public class PurchaseItemDto
{
    public int MedicineId { get; set; }
    public string BatchNumber { get; set; } = "";
    public DateOnly? ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public int FreeQuantity { get; set; }
    public decimal PurchaseRate { get; set; }
    public decimal Mrp { get; set; }
    public decimal SalePrice { get; set; }
    public decimal GstPercentage { get; set; }
    public decimal Amount { get; set; }
}