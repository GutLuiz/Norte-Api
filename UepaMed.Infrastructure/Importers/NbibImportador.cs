using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;

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
            var artigos = new List<Artigo>();

            // Vamos implementar a leitura do NBIB aqui.

            return artigos;
        }
    }
}