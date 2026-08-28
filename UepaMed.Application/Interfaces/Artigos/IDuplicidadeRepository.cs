using UepaMed.Domain.Entities.Artigos;

namespace UepaMed.Application.Interfaces.Artigos
{
    public interface IDuplicidadeRepository
    {
          Task<IReadOnlyList<DuplicidadeIgnorada>> ObterParesIgnoradosAsync(
            int revisaoId);

        Task AdicionarParIgnoradoAsync(
            DuplicidadeIgnorada duplicidadeIgnorada);
      
    }
}
