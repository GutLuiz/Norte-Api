using UepaMed.Application.Dtos;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Enums;

namespace UepaMed.Application.Services
{
    public class ImportacaoArtigosService
    {
        private readonly IImportadorArtigos _importador;
        private readonly IArquivoImportacaoRepository _arquivoRepository;
        private readonly IArtigoRepository _artigoRepository;

        public ImportacaoArtigosService(
          IImportadorArtigos importador,
          IArquivoImportacaoRepository arquivoRepository,
          IArtigoRepository artigoRepository)
        {
            _importador = importador;
            _arquivoRepository = arquivoRepository;
            _artigoRepository = artigoRepository;
        }
        public async Task<List<Artigo>> ImportarAsync(
            int revisaoId,
            Stream arquivo,
            string nomeArquivo)
        {
            var extensao = Path.GetExtension(nomeArquivo);

            if (!_importador.Suporta(extensao))
            {
                throw new ArgumentException(
                    $"O arquivo {nomeArquivo} não é suportado.");
            }

            var artigos = await _importador.ImportarAsync(arquivo);

            foreach (var artigo in artigos)
            {
                artigo.RevisaoId = revisaoId;
            }

            var arquivoImportacao = new ArquivoImportacao
            {
                RevisaoId = revisaoId,
                NomeArquivo = nomeArquivo,
                TipoArquivo = TipoArquivoImportacao.NBIB,
                QuantidadeArtigos = artigos.Count,
                DataImportacao = DateTime.UtcNow
            };

            await _arquivoRepository.AdicionarAsync(arquivoImportacao);

            foreach (var artigo in artigos)
            {
                artigo.ArquivoImportacaoId = arquivoImportacao.Id;

                await _artigoRepository.AdicionarAsync(artigo);
            }

            return artigos;
        }

        public async Task<List<ArquivosListaDto>> ListarArquivosAsync(int revisaoId)
        {
            var arquivos = await _arquivoRepository
                .ListarArquivosPorRevisao(revisaoId);

            var contagens = await _artigoRepository
                .ListarContagemStatusPorArquivoAsync(revisaoId);

            return arquivos.Select(arquivo =>
            {
                var status = contagens
                    .FirstOrDefault(c => c.ArquivoImportacaoId == arquivo.Id);

                return new ArquivosListaDto
                {
                    Id = arquivo.Id,
                    NomeArquivo = arquivo.NomeArquivo,
                    QuantidadeArtigos = arquivo.QuantidadeArtigos,
                    TipoArquivo = arquivo.TipoArquivo,

                    QuantidadeIncluidos = status?.QuantidadeIncluidos ?? 0,
                    QuantidadePendentes = status?.QuantidadePendentes ?? 0,
                    QuantidadeExcluidos = status?.QuantidadeExcluidos ?? 0
                };
            }).ToList();
        }

        public async Task<List<Artigo>> ListarArtigosAsync(int revisaoId)
        {
            return await _artigoRepository.ObterPorRevisaoAsync(revisaoId);
        }

        public async Task MudarStatusArtigo(
        int artigoId,
        StatusArtigo status)
        {
            await _artigoRepository.MudarStatusAsync(
                artigoId,
                status);
        }

        public async Task RemoverAsync(int arquivoImportacaoId)
        {
            var arquivo = await _arquivoRepository
                .ObterPorIdAsync(arquivoImportacaoId);

            if (arquivo == null)
            {
                throw new KeyNotFoundException(
                    "Arquivo de importação não encontrado.");

            }

            await _artigoRepository
                .RemoverPorArquivoImportacaoAsync(arquivoImportacaoId);

            await _arquivoRepository
                .RemoverAsync(arquivo);
        }
    }
}