using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UepaMed.Application.Dtos.Revisao;
using UepaMed.Application.Interfaces.Revisoes;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Entities.Revisoes;
using UepaMed.Domain.Enums.Revisoes;

namespace UepaMed.Application.Services
{
    public class RevisaoService
    {
        private readonly IRevisaoRepository _revisaoRepository;
        private readonly IRevisaoMembroRepository _revisaoMembroRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RevisaoService(
            IRevisaoRepository revisaoRepository,
            IHttpContextAccessor httpContextAccessor,
            IRevisaoMembroRepository revisaoMembroRepository)
        {
            _revisaoRepository = revisaoRepository;
            _httpContextAccessor = httpContextAccessor;
            _revisaoMembroRepository = revisaoMembroRepository;
        }

        public async Task<Revisao> CriarRevisao(CriarRevisaoDto dto)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var revisao = new Revisao
            {
                Titulo = dto.Titulo,
                Tipo = dto.Tipo,
                Dominio = dto.Dominio,
                Descricao = dto.Descricao,
                DataCriacao = DateTime.UtcNow,
                UsuarioId = usuarioId
            };

            await _revisaoRepository.AdicionarAsync(revisao);

            await _revisaoRepository.SalvarAsync();

            var membro = new RevisaoMembro
            {
                RevisaoId = revisao.Id,
                UsuarioId = usuarioId,
                Papel = PapelMembroRevisao.Proprietario
            };

            await _revisaoMembroRepository.AdicionarAsync(membro);
            await _revisaoMembroRepository.SalvarAsync();

            return revisao;
        }

        public async Task<List<RevisaoListaDto>> ListarAsync()
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var membros = await _revisaoMembroRepository
                .ListarRevisoesDoUsuarioAsync(usuarioId);

            return membros.Select(m => new RevisaoListaDto
            {
                Id = m.Revisao.Id,
                Titulo = m.Revisao.Titulo,
                Tipo = m.Revisao.Tipo,
                Dominio = m.Revisao.Dominio,
                Descricao = m.Revisao.Descricao,
                DataCriacao = m.Revisao.DataCriacao,

                Papel = m.Papel
            }).ToList();
        }
        public async Task<Revisao?> AtualizarRevisao(int id, AtualizarRevisaoDto dto)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var revisao = await _revisaoRepository
                .BuscarPorIdEUsuarioAsync(id, usuarioId);

            if (revisao == null)
            {
                return null;
            }

            revisao.Titulo = dto.Titulo;
            revisao.Tipo = dto.Tipo;
            revisao.Dominio = dto.Dominio;
            revisao.Descricao = dto.Descricao;
            revisao.DataAtualizacao = DateTime.UtcNow;

            await _revisaoRepository.SalvarAsync();

            return revisao;
        }

        public async Task<bool> DeletarRevisao(int id)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var revisao = await _revisaoRepository
                .BuscarPorIdEUsuarioAsync(id, usuarioId);

            if (revisao == null)
            {
                return false;
            }

            await _revisaoRepository.RemoverAsync(revisao);

            await _revisaoRepository.SalvarAsync();

            return true;
        }

        public async Task<List<RevisaoMembroDto>> ListarMembrosAsync(
          int revisaoId,
          int usuarioId)
        {
            var pertenceARevisao =
                await _revisaoMembroRepository.ExisteMembroAsync(
                    revisaoId,
                    usuarioId
                );

            if (!pertenceARevisao)
            {
                throw new UnauthorizedAccessException(
                    "Usuário não pertence a esta revisão."
                );
            }

            var membros =
                await _revisaoMembroRepository.ListarMembrosDaRevisaoAsync(
                    revisaoId
                );

            return membros.Select(m => new RevisaoMembroDto
            {
                UsuarioId = m.UsuarioId,
                Nome = m.Usuario.Nome,
                Email = m.Usuario.Email,
                Papel = m.Papel
            }).ToList();
        }


    }
}
