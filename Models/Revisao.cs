using UepaMed.Enums;

namespace UepaMed.Models
{
    public class Revisao
    {
        public Guid Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string Titulo { get; set; } = string.Empty;

        public TipoRevisao Tipo { get; set; }

        public DominioRevisao Dominio { get; set; }

        public string? Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }
    }
}
