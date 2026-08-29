using UepaMed.Domain.Entities.Artigos;
using UepaMed.Domain.Enums.Votacao;

namespace UepaMed.Domain.Entities.Votacoes
{
    public class ConflitoVotacao
    {
        public int Id { get; private set; }

        public int VotacaoId { get; private set; }

        public int ArtigoId { get; private set; }

        public MotivoConflito Motivo { get; private set; }

        public bool Resolvido { get; private set; }

        public OpcaoVoto? DecisaoFinal { get; private set; }

        public int? AvaliadorId { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public DateTime? DataResolucao { get; private set; }

        public Votacao Votacao { get; private set; } = null!;

        public Artigo Artigo { get; private set; } = null!;

        private ConflitoVotacao()
        {
        }

        public ConflitoVotacao(
            int votacaoId,
            int artigoId,
            MotivoConflito motivo)
        {
            if (votacaoId <= 0)
            {
                throw new ArgumentException(
                    "O identificador da votação é inválido.",
                    nameof(votacaoId));
            }

            if (artigoId <= 0)
            {
                throw new ArgumentException(
                    "O identificador do artigo é inválido.",
                    nameof(artigoId));
            }

            if (!Enum.IsDefined(
                typeof(MotivoConflito),
                motivo))
            {
                throw new ArgumentException(
                    "O motivo do conflito é inválido.",
                    nameof(motivo));
            }

            VotacaoId = votacaoId;
            ArtigoId = artigoId;
            Motivo = motivo;
            Resolvido = false;
            DataCriacao = DateTime.UtcNow;
        }

        public void Resolver(
            int avaliadorId,
            OpcaoVoto decisaoFinal)
        {
            if (Resolvido)
            {
                throw new InvalidOperationException(
                    "Este conflito já foi resolvido.");
            }

            if (avaliadorId <= 0)
            {
                throw new ArgumentException(
                    "O identificador do avaliador é inválido.",
                    nameof(avaliadorId));
            }

            if (decisaoFinal == OpcaoVoto.AbsterSe)
            {
                throw new InvalidOperationException(
                    "O avaliador deve incluir ou excluir o artigo.");
            }

            if (decisaoFinal != OpcaoVoto.Incluir &&
                decisaoFinal != OpcaoVoto.Excluir)
            {
                throw new ArgumentException(
                    "A decisão final é inválida.",
                    nameof(decisaoFinal));
            }

            AvaliadorId = avaliadorId;
            DecisaoFinal = decisaoFinal;
            Resolvido = true;
            DataResolucao = DateTime.UtcNow;
        }
    }
}