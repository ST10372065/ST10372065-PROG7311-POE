using Microsoft.EntityFrameworkCore;
using ST10372065_PROG7311.Models;

namespace ST10372065_PROG7311.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Product entity to 'product' table
            modelBuilder.Entity<Product>().ToTable("Product");
        }
    }
}
