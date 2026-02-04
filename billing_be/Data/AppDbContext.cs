using Microsoft.EntityFrameworkCore;
using billing_be.Models;

namespace billing_be.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
    }

}
