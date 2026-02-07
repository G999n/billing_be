using billing_be.Data;
using billing_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    // GET: api/Clients/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null) return NotFound();
        return client;
    }

    // POST: api/Clients
    [HttpPost]
    public async Task<ActionResult<Client>> Create(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    // PUT: api/Clients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client updated)
    {
        if (id != updated.Id) return BadRequest();

        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        // Update fields
        client.Name = updated.Name;
        client.PhoneNumber = updated.PhoneNumber;
        client.Email = updated.Email;
        client.Address = updated.Address;
        client.GstNumber = updated.GstNumber;
        client.IsActive = updated.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Clients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/Clients/search?q=john
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Client>>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q)) return BadRequest();

        // Searches by Name OR Phone Number
        var results = await _context.Clients
            .Where(c => EF.Functions.ILike(c.Name, $"%{q}%") ||
                        EF.Functions.ILike(c.PhoneNumber, $"%{q}%"))
            .ToListAsync();

        return results;
    }
}