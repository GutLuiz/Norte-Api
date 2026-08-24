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

            return arquivos.Select(a => new ArquivosListaDto
            {
                Id = a.Id,
                NomeArquivo = a.NomeArquivo,
                QuantidadeArtigos = a.QuantidadeArtigos,
                TipoArquivo = a.TipoArquivo
            }).ToList();
        }

        public async Task<List<Artigo>> ListarArtigosAsync(int revisaoId)
        {
            return await _artigoRepository.ObterPorRevisaoAsync(revisaoId);
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