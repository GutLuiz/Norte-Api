

using UepaMed.Domain.Enums;

namespace UepaMed.Domain.Entities
{
    public class Artigo
    {
        public int Id { get; set; }

        public int RevisaoId { get; set; }

        public int ArquivoImportacaoId { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Resumo { get; set; }

        public string? Autores { get; set; }

        public string? Revista { get; set; }

        public int? AnoPublicacao { get; set; }

        public string? DOI { get; set; }

        public string? PMID { get; set; }

        public Revisao Revisao { get; set; } = null!;

        public ArquivoImportacao ArquivoImportacao { get; set; } = null!;

        public StatusArtigo Status { get; set; } = StatusArtigo.Pendente;
    }
}
