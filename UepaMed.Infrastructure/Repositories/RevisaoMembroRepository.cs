
using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Enums;
using UepaMed.Infrastructure.Data;

namespace UepaMed.Infrastructure.Repositories
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
    }
}
