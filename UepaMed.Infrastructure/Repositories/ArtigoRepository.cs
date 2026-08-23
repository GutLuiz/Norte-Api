using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
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
    }
}