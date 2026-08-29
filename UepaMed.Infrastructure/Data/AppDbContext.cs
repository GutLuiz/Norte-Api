using Microsoft.EntityFrameworkCore;
using UepaMed.Domain.Entities.Arquivos;
using UepaMed.Domain.Entities.Artigos;
using UepaMed.Domain.Entities.Revisoes;
using UepaMed.Domain.Entities.Usuarios;
using UepaMed.Domain.Entities.Votacoes;

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
        public DbSet<DuplicidadeIgnorada> DuplicidadesIgnoradas { get; set; }
        public DbSet<Votacao> Votacoes { get; set; }

        public DbSet<Voto> Votos { get; set; }

        public DbSet<ConflitoVotacao> ConflitosVotacao { get; set; }

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
            modelBuilder.Entity<Votacao>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Status)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(v => v.DataInicio);

                entity.Property(v => v.DataFinalizacao);

                entity.HasIndex(v => v.RevisaoId);

                entity.HasOne<Revisao>()
                    .WithMany()
                    .HasForeignKey(v => v.RevisaoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(v => v.Votos)
                    .WithOne(v => v.Votacao)
                    .HasForeignKey(v => v.VotacaoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Conflitos)
                    .WithOne(c => c.Votacao)
                    .HasForeignKey(c => c.VotacaoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Voto>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Opcao)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(v => v.DataRegistro)
                    .IsRequired();

                entity.HasIndex(v => new
                {
                    v.VotacaoId,
                    v.ArtigoId,
                    v.UsuarioId
                })
                .IsUnique();

                entity.HasOne(v => v.Artigo)
                    .WithMany()
                    .HasForeignKey(v => v.ArtigoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ConflitoVotacao>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Motivo)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(c => c.DecisaoFinal)
                    .HasConversion<int>();

                entity.Property(c => c.Resolvido)
                    .IsRequired();

                entity.Property(c => c.DataCriacao)
                    .IsRequired();

                entity.Property(c => c.DataResolucao);

                entity.HasIndex(c => new
                {
                    c.VotacaoId,
                    c.ArtigoId
                })
                .IsUnique();

                entity.HasOne(c => c.Artigo)
                    .WithMany()
                    .HasForeignKey(c => c.ArtigoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
