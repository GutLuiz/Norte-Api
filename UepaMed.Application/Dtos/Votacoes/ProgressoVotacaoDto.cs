using UepaMed.Domain.Enums.Votacao;

namespace UepaMed.Application.Dtos.Votacoes
{
    public class ProgressoVotacaoDto
    {
        public int VotacaoId { get; set; }

        public int RevisaoId { get; set; }

        public StatusVotacao Status { get; set; }

        public int TotalArtigos { get; set; }

        public int TotalVotantesObrigatorios { get; set; }

        public int TotalVotosEsperados { get; set; }

        public int TotalVotosRegistrados { get; set; }

        public decimal PercentualGeral { get; set; }

        public bool TodosVotaram { get; set; }

        public List<ProgressoVotanteDto> Votantes { get; set; }
            = new();
    }
}