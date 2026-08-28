using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Application.Dtos.Revisao
{
    public class RevisaoMembroDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public PapelMembroRevisao Papel { get; set; }
    }
}
