using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Dtos;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Enums;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories
{
    public class ArquivoImportacaoRepository : IArquivoImportacaoRepository
    {
        private readonly AppDbContext _context;

        public ArquivoImportacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(ArquivoImportacao arquivo)
        {
            await _context.ArquivosImportacao.AddAsync(arquivo);
            await _context.SaveChangesAsync();
        }

        public async Task<ArquivoImportacao?> ObterPorIdAsync(int id)
        {
            return await _context.ArquivosImportacao
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<ArquivoImportacao>> ListarArquivosPorRevisao(int revisaoId)
        {
            return await _context.ArquivosImportacao
                .Where(a => a.RevisaoId == revisaoId)
                .ToListAsync();
        }

       

        public async Task RemoverAsync(ArquivoImportacao arquivo)
        {
            _context.ArquivosImportacao.Remove(arquivo);
            await _context.SaveChangesAsync();
        }
    }
}