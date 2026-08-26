using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UepaMed.Application.DTOs;
using UepaMed.Application.Services;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes")]
    public class RevisoesController : ControllerBase
    {
        private readonly RevisaoService _revisaoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RevisoesController(RevisaoService revisaoService, IHttpContextAccessor httpContextAccessor)
        {
            _revisaoService = revisaoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarRevisaoDto dto)
        {
            var revisao = await _revisaoService.CriarRevisao(dto);

            return CreatedAtAction(
                nameof(Criar),
                new { id = revisao.Id },
                revisao
            );
        }

        [HttpGet]
        public async Task<IActionResult> ListarRevisoes()
        {
            var revisoes = await _revisaoService.ListarAsync();

            return Ok(revisoes);
        }

        [HttpGet("{revisaoId}/membros")]
        public async Task<IActionResult> ListarMembros(int revisaoId)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return Unauthorized();
            }

                var membros =
                    await _revisaoService.ListarMembrosAsync(
                        revisaoId,
                        usuarioId
                    );

                return Ok(membros);
        }
           
        

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
        int id,
        AtualizarRevisaoDto dto)
        {
            var revisao = await _revisaoService.AtualizarRevisao(id, dto);

            if (revisao == null)
            {
                return NotFound(new
                {
                    mensagem = "Revisão não encontrada."
                });
            }

            return Ok(revisao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _revisaoService.DeletarRevisao(id);

            if (!sucesso)
            {
                return NotFound(new
                {
                    mensagem = "Revisão não encontrada."
                });
            }

            return NoContent();
        }
    }
}
