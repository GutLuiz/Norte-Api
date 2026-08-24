using UepaMed.Domain.Enums;

namespace UepaMed.Domain.Entities
{
    public class RevisaoMembro
    {
        public int Id { get; set; }

        public int RevisaoId { get; set; }

        public int UsuarioId { get; set; }

        public PapelMembroRevisao Papel { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        public Revisao Revisao { get; set; } = null!;

        public Usuario Usuario { get; set; } = null!;
    }
}