using billing_be.Models;
using Microsoft.AspNetCore.Mvc;

namespace billing_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChalanController : Controller
    {
        [HttpPost("calculate-row")]
        public ActionResult<decimal> CalculateRow(decimal rate, int qty, decimal disc, decimal tax)
        {
            // Simple billing logic: (Rate * Qty - Discount) + Tax
            var basePrice = rate * qty;
            var discountAmount = basePrice * (disc / 100);
            var priceAfterDisc = basePrice - discountAmount;
            var finalAmount = priceAfterDisc + (priceAfterDisc * (tax / 100));

            return Ok(finalAmount);
        }

        [HttpPost("save-chalan")]
        public IActionResult SaveChalan([FromBody] List<ChalanItem> items)
        {
            // Logic to save to database via DbContext
            return Ok(new { message = "Chalan saved successfully", count = items.Count });
        }
    }
}
