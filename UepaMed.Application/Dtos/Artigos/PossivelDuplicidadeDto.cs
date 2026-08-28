namespace UepaMed.Application.Dtos.Artigos
{
    public class PossivelDuplicidadeDto
    {
        public ArtigoComparacaoDto ArtigoA { get; set; } = null!;

        public ArtigoComparacaoDto ArtigoB { get; set; } = null!;

        public int PercentualSimilaridade { get; set; }
    }
}