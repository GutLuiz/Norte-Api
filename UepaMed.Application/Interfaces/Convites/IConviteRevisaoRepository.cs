using UepaMed.Domain.Entities.Artigos;

namespace UepaMed.Application.Interfaces.Convites
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