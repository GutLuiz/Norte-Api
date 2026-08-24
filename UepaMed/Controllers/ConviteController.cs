using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Dtos;
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
    }
}