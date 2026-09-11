using UepaMed.Domain.Entities.Artigos;

namespace UepaMed.Domain.Entities.Votacoes
{
    public class VotacaoArtigo
    {
        public int Id { get; private set; }

        public int VotacaoId { get; private set; }

        public int ArtigoId { get; private set; }

        public Votacao Votacao { get; private set; } = null!;

        public Artigo Artigo { get; private set; } = null!;

        private VotacaoArtigo()
        {
        }

        public VotacaoArtigo(int artigoId)
        {
            if (artigoId <= 0)
            {
                throw new ArgumentException(
                    "O identificador do artigo é inválido.",
                    nameof(artigoId));
            }

            ArtigoId = artigoId;
        }
    }
}