using Microsoft.EntityFrameworkCore;
using billing_be.Models;

namespace billing_be.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MedicineRecord> MedicineRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_trgm");

            modelBuilder.Entity<MedicineRecord>()
                .HasIndex(m => m.Name)
                .HasMethod("gin") 
                .HasOperators("gin_trgm_ops");
        }
    }
}
