using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Dtos.Votacoes;
using UepaMed.Application.Services;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/votacoes")]
    public class VotacaoController : ControllerBase
    {
        private readonly VotacaoService
            _votacaoService;

        public VotacaoController(
            VotacaoService votacaoService)
        {
            _votacaoService = votacaoService;
        }

        [HttpPost("iniciar")]
        public async Task<ActionResult<VotacaoRespostaDto>>
            Iniciar(
                [FromBody] IniciarVotacaoDto dto)
        {
            try
            {
                var votacao = await _votacaoService
                    .IniciarAsync(dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        votacaoId = votacao.Id
                    },
                    votacao);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new
                {
                    mensagem = exception.Message
                });
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new
                {
                    mensagem = exception.Message
                });
            }
        }

        [HttpPost("{votacaoId:int}/votos")]
        public async Task<ActionResult<VotoRespostaDto>>
            RegistrarVoto(
                int votacaoId,
                [FromBody] RegistrarVotoDto dto)
        {
            try
            {
                var voto = await _votacaoService
                    .RegistrarVotoAsync(
                        votacaoId,
                        dto);

                return Ok(voto);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new
                {
                    mensagem = exception.Message
                });
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new
                {
                    mensagem = exception.Message
                });
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new
                {
                    mensagem = exception.Message
                });
            }
        }

        [HttpGet("{votacaoId:int}")]
        public async Task<ActionResult<VotacaoRespostaDto>>
            ObterPorId(
                int votacaoId)
        {
            try
            {
                var votacao = await _votacaoService
                    .ObterPorIdAsync(votacaoId);

                return Ok(votacao);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new
                {
                    mensagem = exception.Message
                });
            }
        }

        [HttpGet("revisao/{revisaoId:int}/ativa")]
        public async Task<ActionResult<VotacaoRespostaDto>>
            ObterAtivaPorRevisao(
                int revisaoId)
        {
            var votacao = await _votacaoService
                .ObterAtivaPorRevisaoAsync(
                    revisaoId);

            if (votacao == null)
            {
                return NotFound(new
                {
                    mensagem =
                        "A revisão não possui uma votação ativa."
                });
            }

            return Ok(votacao);
        }
    }
}