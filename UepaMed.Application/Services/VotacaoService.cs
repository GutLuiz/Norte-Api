using UepaMed.Application.Dtos.Votacoes;
using UepaMed.Application.Interfaces.Artigos;
using UepaMed.Application.Interfaces.Votacoes;
using UepaMed.Domain.Entities.Votacoes;

namespace UepaMed.Application.Services
{
    public class VotacaoService
    {
        private readonly IVotacaoRepository
            _votacaoRepository;

        private readonly IArtigoRepository
            _artigoRepository;

        public VotacaoService(
            IVotacaoRepository votacaoRepository,
            IArtigoRepository artigoRepository)
        {
            _votacaoRepository = votacaoRepository;
            _artigoRepository = artigoRepository;
        }

        public async Task<VotacaoRespostaDto> IniciarAsync(
            IniciarVotacaoDto dto)
        {
            if (dto.RevisaoId <= 0)
            {
                throw new ArgumentException(
                    "O identificador da revisão é inválido.",
                    nameof(dto.RevisaoId));
            }

            var votacaoAtiva = await _votacaoRepository
                .ObterAtivaPorRevisaoAsync(
                    dto.RevisaoId);

            if (votacaoAtiva != null)
            {
                throw new InvalidOperationException(
                    "Esta revisão já possui uma votação ativa.");
            }

            var votacao = new Votacao(
                dto.RevisaoId);

            votacao.Iniciar();

            await _votacaoRepository
                .AdicionarAsync(votacao);

            return MapearVotacao(votacao);
        }

        public async Task<VotoRespostaDto>
            RegistrarVotoAsync(
                int votacaoId,
                RegistrarVotoDto dto)
        {
            var votacao = await _votacaoRepository
                .ObterPorIdAsync(votacaoId);

            if (votacao == null)
            {
                throw new KeyNotFoundException(
                    "Votação não encontrada.");
            }

            var artigo = await _artigoRepository
                .ObterPorIdAsync(dto.ArtigoId);

            if (artigo == null)
            {
                throw new KeyNotFoundException(
                    "Artigo não encontrado.");
            }

            if (artigo.RevisaoId !=
                votacao.RevisaoId)
            {
                throw new InvalidOperationException(
                    "O artigo não pertence à revisão desta votação.");
            }

            var voto = votacao.RegistrarVoto(
                dto.ArtigoId,
                dto.UsuarioId,
                dto.Opcao);

            await _votacaoRepository
                .AtualizarAsync(votacao);

            return MapearVoto(voto);
        }

        public async Task<VotacaoRespostaDto>
            ObterPorIdAsync(
                int votacaoId)
        {
            var votacao = await _votacaoRepository
                .ObterPorIdAsync(votacaoId);

            if (votacao == null)
            {
                throw new KeyNotFoundException(
                    "Votação não encontrada.");
            }

            return MapearVotacao(votacao);
        }

        public async Task<VotacaoRespostaDto?>
            ObterAtivaPorRevisaoAsync(
                int revisaoId)
        {
            var votacao = await _votacaoRepository
                .ObterAtivaPorRevisaoAsync(
                    revisaoId);

            if (votacao == null)
            {
                return null;
            }

            return MapearVotacao(votacao);
        }

        private static VotacaoRespostaDto
            MapearVotacao(
                Votacao votacao)
        {
            return new VotacaoRespostaDto
            {
                Id = votacao.Id,
                RevisaoId = votacao.RevisaoId,
                Status = votacao.Status,
                DataInicio = votacao.DataInicio,
                DataFinalizacao =
                    votacao.DataFinalizacao
            };
        }

        private static VotoRespostaDto
            MapearVoto(
                Voto voto)
        {
            return new VotoRespostaDto
            {
                Id = voto.Id,
                VotacaoId = voto.VotacaoId,
                ArtigoId = voto.ArtigoId,
                UsuarioId = voto.UsuarioId,
                Opcao = voto.Opcao,
                DataRegistro = voto.DataRegistro
            };
        }
    }
}