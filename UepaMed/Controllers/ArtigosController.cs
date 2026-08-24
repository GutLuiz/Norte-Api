using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Services;
using UepaMed.Domain.Enums;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes/{revisaoId}/artigos")]
    public class ArtigosController : ControllerBase
    {
        private readonly ImportacaoArtigosService _service;

        public ArtigosController(ImportacaoArtigosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ListarArtigos(int revisaoId)
        {
            var artigos = await _service.ListarArtigosAsync(revisaoId);

            return Ok(artigos);
        }

        [HttpPut("{artigoId}/status")]
        public async Task<IActionResult> MudarStatusArtigo(
            int artigoId,
            [FromBody] StatusArtigo status)
        {
            await _service.MudarStatusArtigo(artigoId, status);

            return NoContent();
        }
    }
}