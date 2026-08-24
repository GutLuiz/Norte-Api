using UepaMed.Domain.Entities;

namespace UepaMed.Application.Interfaces
{
    public interface IConviteRevisaoRepository
    {
        Task AdicionarAsync(ConviteRevisao convite);

        Task<ConviteRevisao?> ObterPorIdAsync(int conviteId);

        Task<List<ConviteRevisao>> ListarPorUsuarioAsync(int usuarioId);

        Task<bool> ExisteConvitePendenteAsync(
            int revisaoId,
            int usuarioConvidadoId);

        Task SalvarAsync();
    }
}