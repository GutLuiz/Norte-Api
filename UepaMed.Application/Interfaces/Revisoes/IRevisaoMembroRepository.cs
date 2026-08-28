using UepaMed.Domain.Entities.Revisoes;

namespace UepaMed.Application.Interfaces.Revisoes
{
    public interface IRevisaoMembroRepository
    {
        Task AdicionarAsync(RevisaoMembro membro);
        Task SalvarAsync();

        Task<bool> ExisteMembroAsync(int revisaoId, int usuarioId);

        Task<bool> EhProprietarioAsync(int revisaoId, int usuarioId);

        Task<List<RevisaoMembro>> ListarRevisoesDoUsuarioAsync(int usuarioId);
        Task<List<RevisaoMembro>> ListarMembrosDaRevisaoAsync(int revisaoId);

    }
}
