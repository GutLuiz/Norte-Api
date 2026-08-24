using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Dtos;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Enums;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories
{
    public class ArtigoRepository : IArtigoRepository
    {
        private readonly AppDbContext _context;

        public ArtigoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Artigo artigo)
        {
            await _context.Artigos.AddAsync(artigo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Artigo>> ObterPorRevisaoAsync(int revisaoId)
        {
            return await _context.Artigos
                .Where(a => a.RevisaoId == revisaoId)
                .ToListAsync();
        }

        public async Task MudarStatusAsync(
         int artigoId,
         StatusArtigo status)
        {
            var artigo = await _context.Artigos
                .FirstOrDefaultAsync(a => a.Id == artigoId);

            if (artigo == null)
                throw new KeyNotFoundException("Artigo não encontrado.");

            artigo.Status = status;

            await _context.SaveChangesAsync();
        }

        public async Task<List<ContagemStatusArquivoDto>>
       ListarContagemStatusPorArquivoAsync(int revisaoId)
        {
            return await _context.Artigos
                .Where(a => a.RevisaoId == revisaoId)
                .GroupBy(a => a.ArquivoImportacaoId)
                .Select(g => new ContagemStatusArquivoDto
                {
                    ArquivoImportacaoId = g.Key,

                    QuantidadeIncluidos = g.Count(a =>
                        a.Status == StatusArtigo.Incluido),

                    QuantidadePendentes = g.Count(a =>
                        a.Status == StatusArtigo.Pendente),

                    QuantidadeExcluidos = g.Count(a =>
                        a.Status == StatusArtigo.Excluido)
                })
                .ToListAsync();
        }
        public async Task RemoverPorArquivoImportacaoAsync(int arquivoImportacaoId)
        {
            var artigos = await _context.Artigos
                .Where(a => a.ArquivoImportacaoId == arquivoImportacaoId)
                .ToListAsync();

            _context.Artigos.RemoveRange(artigos);

            await _context.SaveChangesAsync();
        }
    }
}