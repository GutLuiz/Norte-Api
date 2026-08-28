namespace UepaMed.Application.Dtos
{
    public class ArtigoComparacaoDto
    {
        public int Id { get; set; }

        public int ArquivoImportacaoId { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Resumo { get; set; }

        public string? Autores { get; set; }

        public string? Revista { get; set; }

        public int? AnoPublicacao { get; set; }

        public string? DOI { get; set; }

        public string? PMID { get; set; }
    }
}