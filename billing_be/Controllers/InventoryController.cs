using billing_be.Data;
using billing_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public InventoryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inventory>>> GetAll()
    {
        return await _context.Inventories
            .Include(i => i.Medicine)
            .ToListAsync();
    }

    // GET: api/Inventory/Medicine/{medicineId}
    [HttpGet("Medicine/{medicineId}")]
    public async Task<ActionResult<IEnumerable<Inventory>>> GetByMedicine(int medicineId)
    {
        var inventories = await _context.Inventories
            .Where(i => i.MedicineId == medicineId && i.IsActive)
            .ToListAsync();

        return inventories;
    }

    // POST: api/Inventory
    [HttpPost]
    public async Task<ActionResult<Inventory>> AddStock(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetByMedicine), new { medicineId = inventory.MedicineId }, inventory);
    }

    // PUT: api/Inventory/{id} (update stock, price, etc.)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Inventory updated)
    {
        if (id != updated.Id) return BadRequest();

        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null) return NotFound();

        inventory.BatchNumber = updated.BatchNumber;
        inventory.Mrp = updated.Mrp;
        inventory.PurchasePrice = updated.PurchasePrice;
        inventory.SalePrice = updated.SalePrice;
        inventory.StockQuantity = updated.StockQuantity;
        inventory.ExpiryDate = updated.ExpiryDate;
        inventory.GstPercentage = updated.GstPercentage;
        inventory.IsActive = updated.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Inventory/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null) return NotFound();

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
