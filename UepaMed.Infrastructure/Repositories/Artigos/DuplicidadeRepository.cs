using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces.Artigos;
using UepaMed.Domain.Entities.Artigos;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories.Artigos
{
    public class DuplicidadeRepository : IDuplicidadeRepository
    {
        private readonly AppDbContext _context;

        public DuplicidadeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<DuplicidadeIgnorada>>
            ObterParesIgnoradosAsync(int revisaoId)
        {
            return await _context.DuplicidadesIgnoradas
                .AsNoTracking()
                .Where(d => d.RevisaoId == revisaoId)
                .ToListAsync();
        }

        public async Task AdicionarParIgnoradoAsync(
            DuplicidadeIgnorada duplicidadeIgnorada)
        {
            OrganizarIdsDosArtigos(duplicidadeIgnorada);

            var parJaExiste = await _context.DuplicidadesIgnoradas
                .AnyAsync(d =>
                    d.RevisaoId == duplicidadeIgnorada.RevisaoId &&
                    d.ArtigoAId == duplicidadeIgnorada.ArtigoAId &&
                    d.ArtigoBId == duplicidadeIgnorada.ArtigoBId);

            if (parJaExiste)
                return;

            await _context.DuplicidadesIgnoradas
                .AddAsync(duplicidadeIgnorada);

            await _context.SaveChangesAsync();
        }

        private static void OrganizarIdsDosArtigos(
            DuplicidadeIgnorada duplicidadeIgnorada)
        {
            if (duplicidadeIgnorada.ArtigoAId <
                duplicidadeIgnorada.ArtigoBId)
            {
                return;
            }

            var artigoAId = duplicidadeIgnorada.ArtigoAId;

            duplicidadeIgnorada.ArtigoAId =
                duplicidadeIgnorada.ArtigoBId;

            duplicidadeIgnorada.ArtigoBId = artigoAId;
        }
    }
}