using UepaMed.Application.Dtos.importacao;
using UepaMed.Domain.Entities.Artigos;
using UepaMed.Domain.Enums;

namespace UepaMed.Application.Interfaces.Artigos
{
    public interface IArtigoRepository
    {
        Task AdicionarAsync(Artigo artigo);

        Task<List<Artigo>> ObterPorRevisaoAsync(int revisaoId);

      
        Task<List<ContagemStatusArquivoDto>>ListarContagemStatusPorArquivoAsync(int revisaoId);
        

        Task MudarStatusAsync(int artigoId, StatusArtigo status);

        Task RemoverPorArquivoImportacaoAsync(int arquivoImportacaoId);
    }
}
