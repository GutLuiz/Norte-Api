using UepaMed.Domain.Entities.Artigos;
using UepaMed.Domain.Enums.Votacao;

namespace UepaMed.Domain.Entities.Votacoes
{
    public class Voto
    {
        public int Id { get; private set; }

        public int VotacaoId { get; private set; }

        public int ArtigoId { get; private set; }

        public int UsuarioId { get; private set; }

        public OpcaoVoto Opcao { get; private set; }

        public DateTime DataRegistro { get; private set; }

        public Votacao Votacao { get; private set; } = null!;

        public Artigo Artigo { get; private set; } = null!;

        private Voto()
        {
        }

        public Voto(
            int votacaoId,
            int artigoId,
            int usuarioId,
            OpcaoVoto opcao)
        {
            ValidarIdentificadores(
                votacaoId,
                artigoId,
                usuarioId);

            ValidarOpcao(opcao);

            VotacaoId = votacaoId;
            ArtigoId = artigoId;
            UsuarioId = usuarioId;
            Opcao = opcao;
            DataRegistro = DateTime.UtcNow;
        }

        public void AlterarOpcao(OpcaoVoto novaOpcao)
        {
            ValidarOpcao(novaOpcao);

            Opcao = novaOpcao;
            DataRegistro = DateTime.UtcNow;
        }

        private static void ValidarIdentificadores(
            int votacaoId,
            int artigoId,
            int usuarioId)
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

            if (usuarioId <= 0)
            {
                throw new ArgumentException(
                    "O identificador do usuário é inválido.",
                    nameof(usuarioId));
            }
        }

        private static void ValidarOpcao(OpcaoVoto opcao)
        {
            if (!Enum.IsDefined(typeof(OpcaoVoto), opcao))
            {
                throw new ArgumentException(
                    "A opção de voto é inválida.",
                    nameof(opcao));
            }
        }
    }
}