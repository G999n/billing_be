using billing_be.Data;
using billing_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicinesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Medicines
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
    {
        return await _context.Medicines
            .Include(m => m.Inventories) // optional, include inventory info
            .ToListAsync();
    }

    // GET: api/Medicines/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Medicine>> GetById(int id)
    {
        var medicine = await _context.Medicines
            .Include(m => m.Inventories)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medicine == null) return NotFound();
        return medicine;
    }

    // POST: api/Medicines
    [HttpPost]
    public async Task<ActionResult<Medicine>> Create(Medicine medicine)
    {
        _context.Medicines.Add(medicine);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = medicine.Id }, medicine);
    }

    // PUT: api/Medicines/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Medicine updated)
    {
        if (id != updated.Id) return BadRequest();

        var medicine = await _context.Medicines.FindAsync(id);
        if (medicine == null) return NotFound();

        // Update fields
        medicine.Name = updated.Name;
        medicine.Brand = updated.Brand;
        medicine.Composition = updated.Composition;
        medicine.HsnCode = updated.HsnCode;
        medicine.Category = updated.Category;
        medicine.IsActive = updated.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Medicines/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var medicine = await _context.Medicines.FindAsync(id);
        if (medicine == null) return NotFound();

        _context.Medicines.Remove(medicine);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/Medicines/search?q=paracetamol
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Medicine>>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q)) return BadRequest();

        var results = await _context.Medicines
            .Where(m => EF.Functions.ILike(m.Name, $"%{q}%") ||
                        EF.Functions.ILike(m.Brand, $"%{q}%"))
            .ToListAsync();

        return results;
    }
}
