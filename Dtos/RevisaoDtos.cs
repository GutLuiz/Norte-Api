using UepaMed.Enums;

namespace UepaMed.Dtos
{
    public class CriarRevisaoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public TipoRevisao Tipo { get; set; }
        public DominioRevisao Dominio { get; set; }
        public string? Descricao { get; set; }
    }

    public class RevisaoListaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public TipoRevisao Tipo { get; set; }
        public DominioRevisao Dominio { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class AtualizarRevisaoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public TipoRevisao Tipo { get; set; }
        public DominioRevisao Dominio { get; set; }
        public string? Descricao { get; set; }
    }
}
