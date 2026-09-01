using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UepaMed.Application.Dtos.Convite;
using UepaMed.Application.Interfaces.Convites;
using UepaMed.Application.Interfaces.Revisoes;
using UepaMed.Application.Interfaces.Usuarios;
using UepaMed.Domain.Entities.Artigos;
using UepaMed.Domain.Entities.Revisoes;
using UepaMed.Domain.Enums.Revisoes;
using UepaMed.Application.Interfaces.Votacoes;


namespace UepaMed.Application.Services
{
    public class ConviteRevisaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRevisaoMembroRepository _revisaoMembroRepository;
        private readonly IConviteRevisaoRepository _conviteRevisaoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVotacaoRepository _votacaoRepository;

        public ConviteRevisaoService(
            IUsuarioRepository usuarioRepository,
            IRevisaoMembroRepository revisaoMembroRepository,
            IConviteRevisaoRepository conviteRevisaoRepository,
            IHttpContextAccessor httpContextAccessor,
            IVotacaoRepository votacaoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _revisaoMembroRepository = revisaoMembroRepository;
            _conviteRevisaoRepository = conviteRevisaoRepository;
            _httpContextAccessor = httpContextAccessor;
            _votacaoRepository = votacaoRepository;
        }

        public async Task CriarConviteAsync(
       int revisaoId,
       CriarConviteRevisaoDto dto)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioAtualId))
                throw new UnauthorizedAccessException(
                    "Usuário não autenticado.");

            var ehProprietario = await _revisaoMembroRepository
                .EhProprietarioAsync(
                    revisaoId,
                    usuarioAtualId);

            if (!ehProprietario)
            {
                throw new UnauthorizedAccessException(
                    "Somente o proprietário da revisão pode convidar membros.");
            }

            await GarantirQueRevisaoNaoEstaEmVotacaoAsync(
                revisaoId);
            var papel = ValidarPapelDoConvite(
             dto.Papel);

            var usuarioConvidado = await _usuarioRepository
                .BuscarPorEmailAsync(dto.Email);

            if (usuarioConvidado == null)
                throw new KeyNotFoundException(
                    "Usuário não encontrado.");

            if (usuarioConvidado.Id == usuarioAtualId)
                throw new InvalidOperationException(
                    "Você não pode convidar a si mesmo.");

            var jaEhMembro = await _revisaoMembroRepository
                .ExisteMembroAsync(
                    revisaoId,
                    usuarioConvidado.Id);

            if (jaEhMembro)
                throw new InvalidOperationException(
                    "Este usuário já é membro da revisão.");

            var convitePendente = await _conviteRevisaoRepository
                .ExisteConvitePendenteAsync(
                    revisaoId,
                    usuarioConvidado.Id);

            if (convitePendente)
                throw new InvalidOperationException(
                    "Já existe um convite pendente para este usuário.");

            var convite = new ConviteRevisao
            {
                RevisaoId = revisaoId,
                UsuarioConvidadoId = usuarioConvidado.Id,
                ConvidadoPorUsuarioId = usuarioAtualId,
                Papel = papel,
                Status = StatusConviteRevisao.Pendente,
                CriadoEm = DateTime.UtcNow
            };

            await _conviteRevisaoRepository
                .AdicionarAsync(convite);

            await _conviteRevisaoRepository
                .SalvarAsync();
        }
        public async Task<List<ConviteListaDto>> ListarMeusConvitesAsync()
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            var convites = await _conviteRevisaoRepository
                .ListarPorUsuarioAsync(usuarioId);

            return convites.Select(c => new ConviteListaDto
            {
                Id = c.Id,
                RevisaoId = c.RevisaoId,
                TituloRevisao = c.Revisao.Titulo,
                NomeProprietario = c.ConvidadoPorUsuario.Nome,
                Papel = c.Papel.ToString(),
                Status = c.Status.ToString(),
                CriadoEm = c.CriadoEm,
            }).ToList();


        }
        public async Task AceitarConviteAsync(int conviteId)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            var convite = await _conviteRevisaoRepository
                .ObterPorIdAsync(conviteId);

            if (convite == null)
                throw new KeyNotFoundException("Convite não encontrado.");

            if (convite.UsuarioConvidadoId != usuarioId)
                throw new UnauthorizedAccessException(
                    "Este convite não pertence ao usuário autenticado.");

            if (convite.Status != StatusConviteRevisao.Pendente)
                throw new InvalidOperationException(
                    "Este convite já foi respondido.");

            await GarantirQueRevisaoNaoEstaEmVotacaoAsync(
             convite.RevisaoId);

            var jaEhMembro = await _revisaoMembroRepository
                .ExisteMembroAsync(convite.RevisaoId, usuarioId);

            if (jaEhMembro)
                throw new InvalidOperationException(
                    "Você já é membro desta revisão.");

            var membro = new RevisaoMembro
            {
                RevisaoId = convite.RevisaoId,
                UsuarioId = usuarioId,
                Papel = ValidarPapelDoConvite(
                convite.Papel),
                CriadoEm = DateTime.UtcNow
            };

            await _revisaoMembroRepository.AdicionarAsync(membro);

            convite.Status = StatusConviteRevisao.Aceito;
            convite.RespondidoEm = DateTime.UtcNow;

            await _conviteRevisaoRepository.SalvarAsync();
        }
        public async Task RecusarConviteAsync(int conviteId)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            var convite = await _conviteRevisaoRepository
                .ObterPorIdAsync(conviteId);

            if (convite == null)
                throw new KeyNotFoundException("Convite não encontrado.");

            if (convite.UsuarioConvidadoId != usuarioId)
                throw new UnauthorizedAccessException(
                    "Este convite não pertence ao usuário autenticado.");

            if (convite.Status != StatusConviteRevisao.Pendente)
                throw new InvalidOperationException(
                    "Este convite já foi respondido.");

            convite.Status = StatusConviteRevisao.Recusado;
            convite.RespondidoEm = DateTime.UtcNow;

            await _conviteRevisaoRepository.SalvarAsync();
        }
        private async Task GarantirQueRevisaoNaoEstaEmVotacaoAsync(
        int revisaoId)
        {
            var votacaoAtiva = await _votacaoRepository
                .ObterAtivaPorRevisaoAsync(revisaoId);

            if (votacaoAtiva != null)
            {
                throw new InvalidOperationException(
                    "Não é possível adicionar membros enquanto a revisão está em votação.");
            }
        }
        private static PapelMembroRevisao
        ValidarPapelDoConvite(
        PapelMembroRevisao? papel)
        {
            if (!papel.HasValue)
            {
                throw new ArgumentException(
                    "Informe o papel do usuário convidado.");
            }

            return papel.Value switch
            {
                PapelMembroRevisao.Revisor =>
                    PapelMembroRevisao.Revisor,

                PapelMembroRevisao.Colaborador =>
                    PapelMembroRevisao.Colaborador,

                PapelMembroRevisao.Avaliador =>
                    PapelMembroRevisao.Avaliador,

                _ => throw new ArgumentException(
                    "O papel do convite deve ser Revisor, Colaborador ou Avaliador.")
            };
        }
    }
}
