using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MaterialsApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Material> Materials => Set<Material>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .Property(m => m.AddedOn)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
