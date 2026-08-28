using System.Text.RegularExpressions;
using UepaMed.Application.Interfaces.Artigos;
using UepaMed.Domain.Entities.Artigos;

namespace UepaMed.Infrastructure.Importers
{
    public class NbibImportador : IImportadorArtigos
    {
        public bool Suporta(string extensao)
        {
            return extensao.Equals(".nbib", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<List<Artigo>> ImportarAsync(Stream arquivo)
        {
            using var reader = new StreamReader(arquivo);

            var conteudo = await reader.ReadToEndAsync();
            var artigos = new List<Artigo>();

            var registros = Regex.Split(
            conteudo,
            @"(?=^PMID-)",
            RegexOptions.Multiline
);

            foreach (var registro in registros)
            {
                if (string.IsNullOrWhiteSpace(registro))
                    continue;

                var campos = ExtrairCampos(registro);

                var artigo = new Artigo
                {
                    PMID = campos.GetValueOrDefault("PMID"),
                    Titulo = campos.GetValueOrDefault("TI") ?? string.Empty,
                    Resumo = campos.GetValueOrDefault("AB"),
                    Revista = campos.GetValueOrDefault("JT"),
                    Autores = campos.GetValueOrDefault("FAU"),
                    DOI = campos.GetValueOrDefault("LID"),
                    AnoPublicacao = ExtrairAno(campos.GetValueOrDefault("DP")),

                };

                artigos.Add(artigo);
            }

            return artigos;
        }

        private Dictionary<string, string> ExtrairCampos(string registro)
        {
            var campos = new Dictionary<string, string>();

            string? campoAtual = null;

            var linhas = registro.Split(
                new[] { "\r\n", "\n" },
                StringSplitOptions.None
            );

            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha))
                    continue;

                // Campo NBIB: duas primeiras posições são o código
                if (linha.Length >= 6 && linha[4] == '-' && linha[5] == ' ')
                {
                    campoAtual = linha.Substring(0, 4).Trim();

                    var valor = linha.Substring(6).Trim();

                    if (campos.ContainsKey(campoAtual))
                    {
                        campos[campoAtual] += "; " + valor;
                    }
                    else
                    {
                        campos[campoAtual] = valor;
                    }
                }
                else if (campoAtual != null && linha.StartsWith("      "))
                {
                    // Continuação do campo anterior
                    campos[campoAtual] += " " + linha.Trim();
                }
            }

            return campos;
        }

        private int? ExtrairAno(string? data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            var partes = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (int.TryParse(partes[0], out var ano))
                return ano;

            return null;
        }
    }
}