using UepaMed.Domain.Enums;

namespace UepaMed.Application.Dtos
{
    public class RevisaoMembroDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public PapelMembroRevisao Papel { get; set; }
    }
}
