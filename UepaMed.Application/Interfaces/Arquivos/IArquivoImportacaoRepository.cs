using UepaMed.Application.Dtos;
using UepaMed.Domain.Entities.Arquivos;


namespace UepaMed.Application.Interfaces.Arquivos
{
    public interface IArquivoImportacaoRepository
    {
        Task AdicionarAsync(ArquivoImportacao arquivo);

        Task<ArquivoImportacao?> ObterPorIdAsync(int id);

        Task<List<ArquivoImportacao>> ListarArquivosPorRevisao(int revisaoId);

       
        Task RemoverAsync(ArquivoImportacao arquivo);
    }
}
