using Microsoft.EntityFrameworkCore;
using UepaMed.Domain.Entities;

namespace UepaMed.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // O QUE É ISSO? (acho que é uma propiedade do entity?)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Revisao> Revisoes => Set<Revisao>();
        public DbSet<Artigo> Artigos => Set<Artigo>();
        public DbSet<ArquivoImportacao> ArquivosImportacao { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
