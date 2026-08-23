using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Services;

namespace UepaMed.Controllers
{
    [ApiController]
    [Route("api/revisoes/{revisaoId}/importacoes")]
    public class ImportacaoArtigosController : ControllerBase
    {
        private readonly ImportacaoArtigosService _service;

        public ImportacaoArtigosController(ImportacaoArtigosService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Importar(
            int revisaoId,
            IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Arquivo não enviado.");

            await using var stream = arquivo.OpenReadStream();

            var artigos = await _service.ImportarAsync(
                revisaoId,
                stream,
                arquivo.FileName);

            return Ok(new
            {
                mensagem = "Arquivo importado com sucesso.",
                quantidadeArtigos = artigos.Count,
                artigos
            });
        }
    }
}