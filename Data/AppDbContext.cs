using Microsoft.EntityFrameworkCore;
using UepaMed.Models;

namespace UepaMed.Data
{
    public class AppDbContext : DbContext
    {
        // O QUE É ISSO? (acho que é uma propiedade do entity?)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
