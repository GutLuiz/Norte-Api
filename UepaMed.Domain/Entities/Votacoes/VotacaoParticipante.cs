using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Domain.Entities.Votacoes
{
    public class VotacaoParticipante
    {
        public int Id { get; private set; }

        public int VotacaoId { get; private set; }

        public int UsuarioId { get; private set; }

        public PapelMembroRevisao Papel { get; private set; }

        public bool EhVotanteObrigatorio { get; private set; }

        public Votacao Votacao { get; private set; } = null!;

        private VotacaoParticipante()
        {
        }

        public VotacaoParticipante(
            int usuarioId,
            PapelMembroRevisao papel)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException(
                    "O identificador do usuário é inválido.",
                    nameof(usuarioId));
            }

            if (!Enum.IsDefined(
                typeof(PapelMembroRevisao),
                papel))
            {
                throw new ArgumentException(
                    "O papel do participante é inválido.",
                    nameof(papel));
            }

            UsuarioId = usuarioId;
            Papel = papel;

            EhVotanteObrigatorio =
                papel == PapelMembroRevisao.Proprietario
                || papel == PapelMembroRevisao.Revisor;
        }
    }
}