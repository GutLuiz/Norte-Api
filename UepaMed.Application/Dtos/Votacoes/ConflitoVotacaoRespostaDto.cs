using UepaMed.Domain.Enums.Votacao;

namespace UepaMed.Application.Dtos.Votacoes
{
    public class ConflitoVotacaoRespostaDto
    {
        public int Id { get; set; }

        public int VotacaoId { get; set; }

        public int ArtigoId { get; set; }

        public MotivoConflito Motivo { get; set; }

        public bool Resolvido { get; set; }

        public OpcaoVoto? DecisaoFinal { get; set; }

        public int? AvaliadorId { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataResolucao { get; set; }
    }
}