using System.Text.RegularExpressions;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;

namespace UepaMed.Infrastructure.Importers
{
    public class RisImportador : IImportadorArtigos
    {
        public bool Suporta(string extensao)
        {
            return extensao.Equals(
                ".ris",
                StringComparison.OrdinalIgnoreCase);
        }

        public async Task<List<Artigo>> ImportarAsync(
            Stream arquivo)
        {
            using var reader = new StreamReader(arquivo);

            var artigos = new List<Artigo>();
            var campos = CriarDicionarioDeCampos();

            string? campoAtual = null;

            while (await reader.ReadLineAsync() is { } linha)
            {
                var correspondencia = Regex.Match(
                    linha,
                    @"^(?<campo>[A-Z0-9]{2})\s*-\s?(?<valor>.*)$");

                if (correspondencia.Success)
                {
                    var campo = correspondencia
                        .Groups["campo"]
                        .Value
                        .ToUpperInvariant();

                    var valor = correspondencia
                        .Groups["valor"]
                        .Value
                        .Trim();

                    if (campo == "TY")
                    {
                        // Proteção para registros sem ER antes do próximo TY.
                        if (campos.Count > 0)
                            AdicionarArtigo(artigos, campos);

                        campos = CriarDicionarioDeCampos();
                        campoAtual = campo;

                        AdicionarValor(
                            campos,
                            campo,
                            valor);

                        continue;
                    }

                    if (campo == "ER")
                    {
                        AdicionarArtigo(
                            artigos,
                            campos);

                        campos = CriarDicionarioDeCampos();
                        campoAtual = null;

                        continue;
                    }

                    campoAtual = campo;

                    AdicionarValor(
                        campos,
                        campo,
                        valor);

                    continue;
                }

                AdicionarContinuacao(
                    campos,
                    campoAtual,
                    linha);
            }

            // Proteção para arquivo sem ER no último registro.
            if (campos.Count > 0)
                AdicionarArtigo(artigos, campos);

            return artigos;
        }

        private static Dictionary<string, List<string>>
            CriarDicionarioDeCampos()
        {
            return new Dictionary<string, List<string>>(
                StringComparer.OrdinalIgnoreCase);
        }

        private static void AdicionarValor(
            Dictionary<string, List<string>> campos,
            string campo,
            string valor)
        {
            if (!campos.TryGetValue(
                    campo,
                    out var valores))
            {
                valores = new List<string>();
                campos[campo] = valores;
            }

            if (!string.IsNullOrWhiteSpace(valor))
                valores.Add(valor);
        }

        private static void AdicionarContinuacao(
            Dictionary<string, List<string>> campos,
            string? campoAtual,
            string linha)
        {
            if (campoAtual == null ||
                string.IsNullOrWhiteSpace(linha) ||
                !campos.TryGetValue(
                    campoAtual,
                    out var valores) ||
                valores.Count == 0)
            {
                return;
            }

            valores[^1] =
                $"{valores[^1]} {linha.Trim()}";
        }

        private static void AdicionarArtigo(
            List<Artigo> artigos,
            Dictionary<string, List<string>> campos)
        {
            var titulo = ObterPrimeiroValor(
                campos,
                "TI",
                "T1",
                "CT");

            if (string.IsNullOrWhiteSpace(titulo))
                return;

            var artigo = new Artigo
            {
                Titulo = titulo,

                Resumo = ObterPrimeiroValor(
                    campos,
                    "AB",
                    "N2"),

                Autores = ObterAutores(campos),

                Revista = ObterPrimeiroValor(
                    campos,
                    "JF",
                    "JO",
                    "JA",
                    "T2"),

                AnoPublicacao = ExtrairAno(
                    ObterPrimeiroValor(
                        campos,
                        "PY",
                        "Y1",
                        "DA")),

                DOI = ExtrairDoi(campos),

                PMID = ExtrairPmid(campos)
            };

            artigos.Add(artigo);
        }

        private static string? ObterPrimeiroValor(
            Dictionary<string, List<string>> campos,
            params string[] nomes)
        {
            foreach (var nome in nomes)
            {
                if (campos.TryGetValue(
                        nome,
                        out var valores) &&
                    valores.Count > 0)
                {
                    var primeiroValor = valores
                        .FirstOrDefault(v =>
                            !string.IsNullOrWhiteSpace(v));

                    if (primeiroValor != null)
                        return primeiroValor;
                }
            }

            return null;
        }

        private static IEnumerable<string> ObterValores(
            Dictionary<string, List<string>> campos,
            params string[] nomes)
        {
            foreach (var nome in nomes)
            {
                if (!campos.TryGetValue(
                        nome,
                        out var valores))
                {
                    continue;
                }

                foreach (var valor in valores)
                {
                    if (!string.IsNullOrWhiteSpace(valor))
                        yield return valor;
                }
            }
        }

        private static string? ObterAutores(
            Dictionary<string, List<string>> campos)
        {
            var autores = ObterValores(
                    campos,
                    "AU",
                    "A1")
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return autores.Count == 0
                ? null
                : string.Join("; ", autores);
        }

        private static int? ExtrairAno(string? data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            var correspondencia = Regex.Match(
                data,
                @"\b(18|19|20|21)\d{2}\b");

            if (!correspondencia.Success)
                return null;

            return int.TryParse(
                correspondencia.Value,
                out var ano)
                    ? ano
                    : null;
        }

        private static string? ExtrairDoi(
            Dictionary<string, List<string>> campos)
        {
            var valoresPossiveis = ObterValores(
                campos,
                "DO",
                "DI",
                "UR",
                "M3",
                "N1");

            foreach (var valor in valoresPossiveis)
            {
                var correspondencia = Regex.Match(
                    valor,
                    @"10\.\d{4,9}/[-._;()/:A-Z0-9]+",
                    RegexOptions.IgnoreCase);

                if (!correspondencia.Success)
                    continue;

                return correspondencia
                    .Value
                    .Trim()
                    .TrimEnd('.', ',', ';');
            }

            return null;
        }

        private static string? ExtrairPmid(
            Dictionary<string, List<string>> campos)
        {
            var valoresPossiveis = ObterValores(
                campos,
                "AN",
                "ID",
                "N1",
                "UR");

            foreach (var valor in valoresPossiveis)
            {
                var pmidExplicito = Regex.Match(
                    valor,
                    @"\bPMID\s*[:\-]?\s*(\d{6,9})\b",
                    RegexOptions.IgnoreCase);

                if (pmidExplicito.Success)
                {
                    return pmidExplicito
                        .Groups[1]
                        .Value;
                }

                var urlPubMed = Regex.Match(
                    valor,
                    @"pubmed\.ncbi\.nlm\.nih\.gov/(\d{6,9})",
                    RegexOptions.IgnoreCase);

                if (urlPubMed.Success)
                {
                    return urlPubMed
                        .Groups[1]
                        .Value;
                }
            }

            return null;
        }
    }
}