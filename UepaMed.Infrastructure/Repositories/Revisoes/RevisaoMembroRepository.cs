
using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces.Revisoes;
using UepaMed.Domain.Entities.Revisoes;
using UepaMed.Domain.Enums.Revisoes;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories.Revisoes
{
    public class RevisaoMembroRepository : IRevisaoMembroRepository
    {
        private readonly AppDbContext _context;

        public RevisaoMembroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(RevisaoMembro membro)
        {
            await _context.RevisoesMembro.AddAsync(membro);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteMembroAsync(
        int revisaoId,
        int usuarioId)
        {
            return await _context.RevisoesMembro
                .AnyAsync(rm =>
                    rm.RevisaoId == revisaoId &&
                    rm.UsuarioId == usuarioId);
        }

        public async Task<bool> EhProprietarioAsync(
        int revisaoId,
        int usuarioId)
        {
            return await _context.RevisoesMembro
                .AnyAsync(rm =>
                    rm.RevisaoId == revisaoId &&
                    rm.UsuarioId == usuarioId &&
                    rm.Papel == PapelMembroRevisao.Proprietario);
        }
        public async Task<List<RevisaoMembro>> ListarRevisoesDoUsuarioAsync(int usuarioId)
        {
            return await _context.RevisoesMembro
                .Where(rm => rm.UsuarioId == usuarioId)
                .Include(rm => rm.Revisao)
                .ToListAsync();
        }

        public async Task<List<RevisaoMembro>> ListarMembrosDaRevisaoAsync(int revisaoId)
        {
            return await _context.RevisoesMembro
                .Where(rm => rm.RevisaoId == revisaoId)
                .Include(rm => rm.Usuario)
                .ToListAsync();
        }
    }
}
