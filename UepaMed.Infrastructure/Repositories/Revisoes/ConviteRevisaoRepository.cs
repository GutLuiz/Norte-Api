using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces.Convites;
using UepaMed.Domain.Entities.Artigos;
using UepaMed.Domain.Enums.Revisoes;
using UepaMed.Infrastructure.Data;

public class ConviteRevisaoRepository : IConviteRevisaoRepository
{
    private readonly AppDbContext _context;

    public ConviteRevisaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(ConviteRevisao convite)
    {
        await _context.ConvitesRevisao.AddAsync(convite);
    }

    public async Task<ConviteRevisao?> ObterPorIdAsync(int conviteId)
    {
        return await _context.ConvitesRevisao
            .FirstOrDefaultAsync(c => c.Id == conviteId);
    }

    public async Task<List<ConviteRevisao>> ListarPorUsuarioAsync(int usuarioId)
    {
        return await _context.ConvitesRevisao
            .Where(c => c.UsuarioConvidadoId == usuarioId)
            .Include(c => c.Revisao)
            .Include(c => c.ConvidadoPorUsuario)
            .OrderByDescending(c => c.CriadoEm)
            .ToListAsync();
    }

    public async Task<bool> ExisteConvitePendenteAsync(
        int revisaoId,
        int usuarioConvidadoId)
    {
        return await _context.ConvitesRevisao
            .AnyAsync(c =>
                c.RevisaoId == revisaoId &&
                c.UsuarioConvidadoId == usuarioConvidadoId &&
                c.Status == StatusConviteRevisao.Pendente);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}