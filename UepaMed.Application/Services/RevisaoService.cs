using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UepaMed.Application.DTOs;
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Domain.Enums;

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

            var revisoes = await _revisaoRepository
                .ListarPorUsuarioAsync(usuarioId);

            return revisoes.Select(r => new RevisaoListaDto
            {
                Id = r.Id,
                Titulo = r.Titulo,
                Tipo = r.Tipo,
                Dominio = r.Dominio,
                Descricao = r.Descricao,
                DataCriacao = r.DataCriacao
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


    }
}
