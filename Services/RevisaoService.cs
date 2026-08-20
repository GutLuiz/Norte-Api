using UepaMed.Data;
using UepaMed.Dtos;
using UepaMed.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace UepaMed.Services
{
    public class RevisaoService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RevisaoService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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

            _context.Revisoes.Add(revisao);

            await _context.SaveChangesAsync();

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

            return await _context.Revisoes
                .Where(r => r.UsuarioId == usuarioId)
                .Select(r => new RevisaoListaDto
                {
                    Id = r.Id,
                    Titulo = r.Titulo,
                    Tipo = r.Tipo,
                    Dominio = r.Dominio,
                    Descricao = r.Descricao,
                    DataCriacao = r.DataCriacao
                })
                .ToListAsync();
        }

        public async Task<Revisao?> AtualizarRevisao(Guid id, AtualizarRevisaoDto dto)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var revisao = await _context.Revisoes
                .FirstOrDefaultAsync(r => r.Id == id && r.UsuarioId == usuarioId);

            if (revisao == null)
            {
                return null;
            }

            revisao.Titulo = dto.Titulo;
            revisao.Tipo = dto.Tipo;
            revisao.Dominio = dto.Dominio;
            revisao.Descricao = dto.Descricao;
            revisao.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return revisao;
        }

        public async Task<bool> DeletarRevisao(Guid id)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var revisao = await _context.Revisoes
                .FirstOrDefaultAsync(r => r.Id == id && r.UsuarioId == usuarioId);

            if (revisao == null)
            {
                return false;
            }

            _context.Revisoes.Remove(revisao);

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
