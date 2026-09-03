using UepaMed.Domain.Entities.Revisoes;
using UepaMed.Domain.Entities.Usuarios;
using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Domain.Entities.Artigos
{
    public class ConviteRevisao
    {
        public int Id { get; set; }

        public int RevisaoId { get; set; }

        public int UsuarioConvidadoId { get; set; }

        public int ConvidadoPorUsuarioId { get; set; }
        public PapelMembroRevisao Papel { get; set; }
        = PapelMembroRevisao.Revisor;

        public StatusConviteRevisao Status { get; set; }
            = StatusConviteRevisao.Pendente;

        public DateTime CriadoEm { get; set; }
            = DateTime.UtcNow;

        public DateTime? RespondidoEm { get; set; }

        public Revisao Revisao { get; set; } = null!;

        public Usuario UsuarioConvidado { get; set; } = null!;

        public Usuario ConvidadoPorUsuario { get; set; } = null!;
    }
}