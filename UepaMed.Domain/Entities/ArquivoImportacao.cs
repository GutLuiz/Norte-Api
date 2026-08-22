using UepaMed.Domain.Enums;

namespace UepaMed.Domain.Entities
{
    public class ArquivoImportacao
    {
        public int Id { get; set; }

        public int RevisaoId { get; set; }

        public string NomeArquivo { get; set; } = string.Empty;

        public TipoArquivoImportacao TipoArquivo { get; set; }

        public int QuantidadeArtigos { get; set; }

        public DateTime DataImportacao { get; set; }

        public Revisao Revisao { get; set; } = null!;

        public ICollection<Artigo> Artigos { get; set; } = new List<Artigo>();
    }
}
