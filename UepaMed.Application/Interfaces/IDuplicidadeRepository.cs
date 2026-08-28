using UepaMed.Domain.Entities;

namespace UepaMed.Application.Interfaces
{
    public interface IDuplicidadeRepository
    {
          Task<IReadOnlyList<DuplicidadeIgnorada>> ObterParesIgnoradosAsync(
            int revisaoId);

        Task AdicionarParIgnoradoAsync(
            DuplicidadeIgnorada duplicidadeIgnorada);
      
    }
}
