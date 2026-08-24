using UepaMed.Domain.Entities;

namespace UepaMed.Application.Interfaces
{
    public interface IRevisaoMembroRepository
    {
        Task AdicionarAsync(RevisaoMembro membro);
        Task SalvarAsync();

        Task<bool> ExisteMembroAsync(int revisaoId, int usuarioId);

        Task<bool> EhProprietarioAsync(int revisaoId, int usuarioId);
    }
}
