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
        public DbSet<RevisaoMembro> RevisoesMembro => Set<RevisaoMembro>();
        public DbSet<Artigo> Artigos => Set<Artigo>();
        public DbSet<ArquivoImportacao> ArquivosImportacao { get; set; }
        public DbSet<ConviteRevisao> ConvitesRevisao { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<ArquivoImportacao>()
             .HasOne(a => a.Revisao)
             .WithMany()
             .HasForeignKey(a => a.RevisaoId)
             .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Artigo>()
            .HasOne(a => a.ArquivoImportacao)
            .WithMany(ai => ai.Artigos)
            .HasForeignKey(a => a.ArquivoImportacaoId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RevisaoMembro>()
            .HasOne(rm => rm.Revisao)
            .WithMany()
            .HasForeignKey(rm => rm.RevisaoId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ConviteRevisao>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.Revisao)
                    .WithMany()
                    .HasForeignKey(c => c.RevisaoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.UsuarioConvidado)
                    .WithMany()
                    .HasForeignKey(c => c.UsuarioConvidadoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.ConvidadoPorUsuario)
                    .WithMany()
                    .HasForeignKey(c => c.ConvidadoPorUsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => new
                {
                    c.RevisaoId,
                    c.UsuarioConvidadoId,
                    c.Status
                });
            });
        }



    }
}
