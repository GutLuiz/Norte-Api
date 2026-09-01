using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces.Revisoes;
using UepaMed.Domain.Entities.Revisoes;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories.Revisoes
{
    public class RevisaoRepository : IRevisaoRepository
    {
        private readonly AppDbContext _context;

        public RevisaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Revisao revisao)
        {
            await _context.Revisoes.AddAsync(revisao);
        }

        public async Task<List<Revisao>> ListarPorUsuarioAsync(int usuarioId)
        {
            return await _context.Revisoes
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Revisao?> BuscarPorIdEUsuarioAsync(int id, int usuarioId)
        {
            return await _context.Revisoes
                .FirstOrDefaultAsync(r =>
                    r.Id == id &&
                    r.UsuarioId == usuarioId);
        }

        public async Task RemoverAsync(
       Revisao revisao)
        {
            var votacoes = await _context.Votacoes
                .Where(votacao =>
                    votacao.RevisaoId == revisao.Id)
                .ToListAsync();

            _context.Votacoes.RemoveRange(
                votacoes);

            _context.Revisoes.Remove(
                revisao);
        }
        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}