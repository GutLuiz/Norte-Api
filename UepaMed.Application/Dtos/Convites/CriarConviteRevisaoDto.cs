using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Application.Dtos.Convite
{
    public class CriarConviteRevisaoDto
    {
        public string Email { get; set; }
            = string.Empty;

        public PapelMembroRevisao? Papel { get; set; }
    }
}