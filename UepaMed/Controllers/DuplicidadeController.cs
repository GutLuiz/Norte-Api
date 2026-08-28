using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Services;

namespace UepaMed.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes/{revisaoId:int}/duplicidades")]
    public class DuplicidadesController : ControllerBase
    {
        private readonly DuplicidadeService _duplicidadeService;

        public DuplicidadesController(
            DuplicidadeService duplicidadeService)
        {
            _duplicidadeService = duplicidadeService;
        }

        [HttpGet("detectar")]
        public async Task<IActionResult> DetectarAsync(
            int revisaoId)
        {
            var resultados = await _duplicidadeService
                .DetectarAsync(revisaoId);

            return Ok(new
            {
                revisaoId,
                quantidadeEncontrada = resultados.Count,

                muitoProvaveis = resultados.Where(
                    r => r.PercentualSimilaridade >= 90),

                provaveis = resultados.Where(
                    r => r.PercentualSimilaridade >= 70 &&
                         r.PercentualSimilaridade < 90),

                poucoProvaveis = resultados.Where(
                    r => r.PercentualSimilaridade >= 50 &&
                         r.PercentualSimilaridade < 70)
            });
        }
    }
}