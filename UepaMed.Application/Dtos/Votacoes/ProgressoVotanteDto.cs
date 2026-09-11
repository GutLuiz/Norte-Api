using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Application.Dtos.Votacoes
{
    public class ProgressoVotanteDto
    {
        public int UsuarioId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public PapelMembroRevisao Papel { get; set; }

        public int VotosRealizados { get; set; }

        public int VotosEsperados { get; set; }

        public int QuantidadeRestante { get; set; }

        public decimal Percentual { get; set; }

        public bool Concluido { get; set; }
    }
}