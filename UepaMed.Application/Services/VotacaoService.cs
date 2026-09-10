using UepaMed.Application.Dtos.Votacoes;
using UepaMed.Application.Interfaces.Artigos;
using UepaMed.Application.Interfaces.Revisoes;
using UepaMed.Application.Interfaces.Votacoes;
using UepaMed.Domain.Entities.Votacoes;
using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Application.Services
{
    public class VotacaoService
    {
        private readonly IVotacaoRepository
            _votacaoRepository;

        private readonly IArtigoRepository
            _artigoRepository;
        private readonly IRevisaoMembroRepository
            _revisaoMembroRepository;

        public VotacaoService(
            IVotacaoRepository votacaoRepository,
            IArtigoRepository artigoRepository,
            IRevisaoMembroRepository revisaoMembroRepository)
        {
            _votacaoRepository = votacaoRepository;
            _artigoRepository = artigoRepository;
            _revisaoMembroRepository = revisaoMembroRepository;

        }

        public async Task<VotacaoRespostaDto> IniciarAsync(
            IniciarVotacaoDto dto)
        {
            var membros = await _revisaoMembroRepository
            .ListarMembrosDaRevisaoAsync(dto.RevisaoId);

            var temProprietario = membros.Any(membro =>
                membro.Papel == PapelMembroRevisao.Proprietario);

            if (!temProprietario)
            {
                throw new InvalidOperationException(
                    "Não é possível iniciar a votação sem um proprietário na revisão.");
            }

            var temRevisor = membros.Any(membro =>
                membro.Papel == PapelMembroRevisao.Revisor);

            var temAvaliador = membros.Any(membro =>
                membro.Papel == PapelMembroRevisao.Avaliador);

            if (temRevisor && !temAvaliador)
            {
                throw new InvalidOperationException(
                    "Não é possível iniciar a votação com revisor sem um avaliador na revisão.");
            }

            if (temAvaliador && !temRevisor)
            {
                throw new InvalidOperationException(
                    "Não é possível iniciar a votação com avaliador sem um revisor na revisão.");
            }

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

            var artigos = await _artigoRepository
            .ObterPorRevisaoAsync(dto.RevisaoId);

            if (artigos.Count == 0)
            {
                throw new InvalidOperationException(
                    "Não é possível iniciar a votação porque a revisão não possui artigos.");
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
        public async Task<List<VotoRespostaDto>>
        ObterMeusVotosAsync(
            int votacaoId,
            int usuarioId)
        {
            if (votacaoId <= 0)
            {
                throw new ArgumentException(
                    "O identificador da votação é inválido.",
                    nameof(votacaoId));
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException(
                    "O identificador do usuário é inválido.",
                    nameof(usuarioId));
            }

            var votacao = await _votacaoRepository
                .ObterPorIdAsync(votacaoId);

            if (votacao == null)
            {
                throw new KeyNotFoundException(
                    "Votação não encontrada.");
            }

            return votacao.Votos
                .Where(voto =>
                    voto.UsuarioId == usuarioId)
                .OrderBy(voto =>
                    voto.ArtigoId)
                .Select(MapearVoto)
                .ToList();
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