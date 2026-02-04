using System.Text.RegularExpressions;
using billing_be.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestController : Controller
    {
        private readonly AppDbContext _context;

        // The "Injection" happens here in the constructor
        public SuggestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest();

            query = Regex.Replace(query.ToLower(), "[^a-z0-9]", "");

            var results = await _context.MedicineRecords
                .Where(m => EF.Functions.ILike(m.NameClean, $"%{query}%")) // Your ILIKE condition
                .Select(m => new
                {
                    m.Name,
                    // Translation of similarity(name, 'query')
                    Score = EF.Functions.TrigramsSimilarity(m.NameClean, query)
                })
                .OrderByDescending(x => x.Score)
                .Take(10)
                .ToListAsync();

            return Ok(results);
        }
    }
}
