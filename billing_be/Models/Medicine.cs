using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace billing_be.Models;

public class Medicine
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(200)]
    public string? Brand { get; set; }

    [MaxLength(200)]
    public string? Composition { get; set; }

    [MaxLength(50)]
    public string? HsnCode { get; set; }

    [MaxLength(50)]
    public string? Category { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
