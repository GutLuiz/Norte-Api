using UepaMed.Application.Dtos.Artigos;
using UepaMed.Domain.Enums.Votacao;

namespace UepaMed.Application.Dtos.Votacoes
{
    public class ArtigoConflitoRespostaDto
    {
        public int ConflitoId { get; set; }

        public int VotacaoId { get; set; }

        public int ArtigoId { get; set; }

        public MotivoConflito Motivo { get; set; }

        public bool Resolvido { get; set; }

        public ArtigoComparacaoDto Artigo { get; set; } = null!;
    }
}