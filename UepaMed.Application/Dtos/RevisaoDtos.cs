using UepaMed.Domain.Enums;

namespace UepaMed.Application.DTOs
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
        public int Id { get; set; }
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
