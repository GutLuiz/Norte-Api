using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UepaMed.Application.Dtos;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Enums;

namespace UepaMed.Application.Services
{
    public class DuplicidadeService
    {
        private readonly IArtigoRepository _artigoRepository;
        private readonly IDuplicidadeRepository _duplicidadeRepository;

        public DuplicidadeService(
            IArtigoRepository artigoRepository,
            IDuplicidadeRepository duplicidadeRepository)
        {
            _artigoRepository = artigoRepository;
            _duplicidadeRepository = duplicidadeRepository;
        }

        public async Task<IReadOnlyList<PossivelDuplicidadeDto>>
            DetectarAsync(int revisaoId)
        {
            var artigos = await _artigoRepository
                .ObterPorRevisaoAsync(revisaoId);

            var artigosDisponiveis = artigos
                .Where(a => a.Status != StatusArtigo.Excluido)
                .ToList();

            var paresIgnorados = await _duplicidadeRepository
                .ObterParesIgnoradosAsync(revisaoId);

            var chavesIgnoradas = paresIgnorados
                .Select(p => CriarChaveDoPar(
                    p.ArtigoAId,
                    p.ArtigoBId))
                .ToHashSet();

            var resultados = new List<PossivelDuplicidadeDto>();

            for (var i = 0; i < artigosDisponiveis.Count; i++)
            {
                for (var j = i + 1; j < artigosDisponiveis.Count; j++)
                {
                    var artigoA = artigosDisponiveis[i];
                    var artigoB = artigosDisponiveis[j];

                    if (artigoA.ArquivoImportacaoId ==
                        artigoB.ArquivoImportacaoId)
                    {
                        continue;
                    }

                    var chaveDoPar = CriarChaveDoPar(
                        artigoA.Id,
                        artigoB.Id);

                    if (chavesIgnoradas.Contains(chaveDoPar))
                        continue;

                    var percentual = CalcularSimilaridade(
                        artigoA,
                        artigoB);

                    if (percentual < 50)
                        continue;

                    resultados.Add(new PossivelDuplicidadeDto
                    {
                        ArtigoA = CriarArtigoDto(artigoA),
                        ArtigoB = CriarArtigoDto(artigoB),
                        PercentualSimilaridade = percentual
                    });
                }
            }

            return resultados
                .OrderByDescending(r => r.PercentualSimilaridade)
                .ToList();
        }

        private static int CalcularSimilaridade(
            Artigo artigoA,
            Artigo artigoB)
        {
            var pmidA = NormalizarPmid(artigoA.PMID);
            var pmidB = NormalizarPmid(artigoB.PMID);

            if (PossuemValoresDiferentes(pmidA, pmidB))
                return 0;

            if (PossuemMesmoValor(pmidA, pmidB))
                return 100;

            var doiA = NormalizarDoi(artigoA.DOI);
            var doiB = NormalizarDoi(artigoB.DOI);

            if (PossuemValoresDiferentes(doiA, doiB))
                return 0;

            if (PossuemMesmoValor(doiA, doiB))
                return 100;

            var similaridadeTitulo = CalcularSimilaridadeTextual(
                artigoA.Titulo,
                artigoB.Titulo);

            if (similaridadeTitulo < 0.55)
                return 0;

            var pontuacao = similaridadeTitulo * 70;

            var similaridadeAutores = CalcularSimilaridadeTextual(
                artigoA.Autores,
                artigoB.Autores);

            pontuacao += similaridadeAutores * 15;

            if (artigoA.AnoPublicacao.HasValue &&
                artigoB.AnoPublicacao.HasValue &&
                artigoA.AnoPublicacao == artigoB.AnoPublicacao)
            {
                pontuacao += 10;
            }

            var similaridadeRevista = CalcularSimilaridadeTextual(
                artigoA.Revista,
                artigoB.Revista);

            if (similaridadeRevista >= 0.80)
                pontuacao += 5;

            return Math.Min(
                100,
                (int)Math.Round(pontuacao));
        }

        private static double CalcularSimilaridadeTextual(
            string? textoA,
            string? textoB)
        {
            var textoNormalizadoA = NormalizarTexto(textoA);
            var textoNormalizadoB = NormalizarTexto(textoB);

            if (string.IsNullOrWhiteSpace(textoNormalizadoA) ||
                string.IsNullOrWhiteSpace(textoNormalizadoB))
            {
                return 0;
            }

            if (textoNormalizadoA == textoNormalizadoB)
                return 1;

            var palavrasA = textoNormalizadoA
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();

            var palavrasB = textoNormalizadoB
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();

            var quantidadeUniao = palavrasA
                .Union(palavrasB)
                .Count();

            if (quantidadeUniao == 0)
                return 0;

            var quantidadeIntersecao = palavrasA
                .Intersect(palavrasB)
                .Count();

            return (double)quantidadeIntersecao /
                   quantidadeUniao;
        }

        private static string NormalizarTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var textoDecomposto = texto
                .Trim()
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD);

            var resultado = new StringBuilder();

            foreach (var caractere in textoDecomposto)
            {
                var categoria = CharUnicodeInfo
                    .GetUnicodeCategory(caractere);

                if (categoria != UnicodeCategory.NonSpacingMark)
                    resultado.Append(caractere);
            }

            var semPontuacao = Regex.Replace(
                resultado.ToString(),
                @"[^\p{L}\p{N}\s]",
                " ");

            return Regex.Replace(
                semPontuacao,
                @"\s+",
                " ").Trim();
        }

        private static string NormalizarPmid(string? pmid)
        {
            if (string.IsNullOrWhiteSpace(pmid))
                return string.Empty;

            return Regex.Replace(pmid, @"\D", "");
        }

        private static string NormalizarDoi(string? doi)
        {
            if (string.IsNullOrWhiteSpace(doi))
                return string.Empty;

            var resultado = doi
                .Trim()
                .ToLowerInvariant()
                .Replace("https://doi.org/", "")
                .Replace("http://doi.org/", "")
                .Replace("doi:", "")
                .Replace("[doi]", "")
                .Trim();

            return resultado;
        }

        private static bool PossuemMesmoValor(
            string valorA,
            string valorB)
        {
            return !string.IsNullOrWhiteSpace(valorA) &&
                   !string.IsNullOrWhiteSpace(valorB) &&
                   valorA == valorB;
        }

        private static bool PossuemValoresDiferentes(
            string valorA,
            string valorB)
        {
            return !string.IsNullOrWhiteSpace(valorA) &&
                   !string.IsNullOrWhiteSpace(valorB) &&
                   valorA != valorB;
        }

        private static string CriarChaveDoPar(
            int artigoAId,
            int artigoBId)
        {
            var menorId = Math.Min(artigoAId, artigoBId);
            var maiorId = Math.Max(artigoAId, artigoBId);

            return $"{menorId}:{maiorId}";
        }

        private static ArtigoComparacaoDto CriarArtigoDto(
            Artigo artigo)
        {
            return new ArtigoComparacaoDto
            {
                Id = artigo.Id,
                ArquivoImportacaoId =
                    artigo.ArquivoImportacaoId,
                Titulo = artigo.Titulo,
                Resumo = artigo.Resumo,
                Autores = artigo.Autores,
                Revista = artigo.Revista,
                AnoPublicacao = artigo.AnoPublicacao,
                DOI = artigo.DOI,
                PMID = artigo.PMID
            };
        }
    }
}