using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Services;

namespace UepaMed.Controllers
{
    [Authorize]
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
               // artigos
            });
        }
        [HttpGet]
        public async Task<IActionResult> ListarImportacoes(int revisaoId)
        {
            var importacoes = await _service.ListarArquivosAsync(revisaoId);

            return Ok(importacoes);
        }

        [HttpGet("artigos")]
        public async Task<IActionResult> ListarArtigos(int revisaoId)
        {
            var importacoes = await _service.ListarArtigosAsync(revisaoId);

            return Ok(importacoes);
        }

        [HttpDelete("{arquivoImportacaoId}")]
        public async Task<IActionResult> Remover(
            int revisaoId,
            int arquivoImportacaoId)
        {
            await _service.RemoverAsync(arquivoImportacaoId);

            return Ok(new
            {
                mensagem = "Arquivo e artigos relacionados removidos com sucesso."
            });
        }
    }
}