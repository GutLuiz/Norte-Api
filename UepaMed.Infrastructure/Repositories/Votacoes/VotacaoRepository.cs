using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces.Votacoes;
using UepaMed.Domain.Entities.Votacoes;
using UepaMed.Domain.Enums.Votacao;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories.Votacoes
{
    public class VotacaoRepository : IVotacaoRepository
    {
        private readonly AppDbContext _context;

        public VotacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(
            Votacao votacao)
        {
            await _context.Votacoes.AddAsync(votacao);

            await _context.SaveChangesAsync();
        }

        public async Task<Votacao?> ObterPorIdAsync(
            int votacaoId)
        {
            return await _context.Votacoes
                .Include(v => v.Votos)
                .Include(v => v.Conflitos)
                .FirstOrDefaultAsync(v =>
                    v.Id == votacaoId);
        }

        public async Task<Votacao?>
            ObterAtivaPorRevisaoAsync(
                int revisaoId)
        {
            return await _context.Votacoes
                .Include(v => v.Votos)
                .Include(v => v.Conflitos)
                .FirstOrDefaultAsync(v =>
                    v.RevisaoId == revisaoId &&
                    v.Status != StatusVotacao.Finalizada);
        }

        public async Task AtualizarAsync(
            Votacao votacao)
        {
            _context.Votacoes.Update(votacao);

            await _context.SaveChangesAsync();
        }
    }
}