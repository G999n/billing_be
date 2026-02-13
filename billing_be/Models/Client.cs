using System.ComponentModel.DataAnnotations;

namespace billing_be.Models;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!; // This acts as "Party Name"

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? GstNumber { get; set; }

    // 👇 NEW FIELD ADDED HERE
    [MaxLength(50)]
    public string? DrugLicense { get; set; }

    public bool IsActive { get; set; } = true;
}