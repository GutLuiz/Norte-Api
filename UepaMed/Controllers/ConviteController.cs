using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Dtos.Convite;
using UepaMed.Application.Services;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes/{revisaoId}/convites")]
    public class ConvitesController : ControllerBase
    {
        private readonly ConviteRevisaoService _service;

        public ConvitesController(ConviteRevisaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarConvite(
            int revisaoId,
            [FromBody] CriarConviteRevisaoDto dto)
        {
            await _service.CriarConviteAsync(revisaoId, dto);

            return Ok(new
            {
                mensagem = "Convite enviado com sucesso."
            });
        }

        [HttpGet("/api/convites")]
        public async Task<IActionResult> ListarMeusConvites()
        {
            var convites = await _service.ListarMeusConvitesAsync();

            return Ok(convites);
        }

        [HttpPost("/api/convites/{conviteId}/aceitar")]
        public async Task<IActionResult> AceitarConvite(int conviteId)
        {
            await _service.AceitarConviteAsync(conviteId);

            return Ok(new
            {
                mensagem = "Convite aceito com sucesso."
            });
        }

        [HttpPost("/api/convites/{conviteId}/recusar")]
        public async Task<IActionResult> RecusarConvite(int conviteId)
        {
            await _service.RecusarConviteAsync(conviteId);

            return Ok(new
            {
                mensagem = "Convite recusado com sucesso."
            });
        }
    }
}