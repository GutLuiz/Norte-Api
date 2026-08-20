using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Dtos;
using UepaMed.Services;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes")]
    public class RevisoesController : ControllerBase
    {
        private readonly RevisaoService _revisaoService;

        public RevisoesController(RevisaoService revisaoService)
        {
            _revisaoService = revisaoService;
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
        public async Task<IActionResult> Listar()
        {
            var revisoes = await _revisaoService.ListarAsync();

            return Ok(revisoes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
        Guid id,
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
        public async Task<IActionResult> Deletar(Guid id)
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
