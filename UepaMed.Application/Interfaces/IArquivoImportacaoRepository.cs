using UepaMed.Application.Dtos;
using UepaMed.Domain.Entities;


namespace UepaMed.Application.Interfaces
{
    public interface IArquivoImportacaoRepository
    {
        Task AdicionarAsync(ArquivoImportacao arquivo);

        Task<ArquivoImportacao?> ObterPorIdAsync(int id);

        Task<List<ArquivoImportacao>> ListarArquivosPorRevisao(int revisaoId);

       
        Task RemoverAsync(ArquivoImportacao arquivo);
    }
}
