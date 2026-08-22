using UepaMed.Domain.Entities;


namespace UepaMed.Application.Interfaces
{
    public interface IArquivoImportacaoRepository
    {
        Task AdicionarAsync(ArquivoImportacao arquivo);

        Task<ArquivoImportacao?> ObterPorIdAsync(int id);

        Task<List<ArquivoImportacao>> ObterPorRevisaoAsync(int revisaoId);

        Task RemoverAsync(ArquivoImportacao arquivo);
    }
}
