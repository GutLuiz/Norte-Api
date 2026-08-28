using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Services;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes/{revisaoId:int}/importacoes")]
    public class ArquivosController : ControllerBase
    {
        private readonly ImportacaoArtigosService _service;

        public ArquivosController(
            ImportacaoArtigosService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> ImportarArquivo(
            int revisaoId,
            [FromForm] IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest(
                    "Arquivo não enviado.");
            }

            var extensao = Path
                .GetExtension(arquivo.FileName)
                .ToLowerInvariant();

            var formatoPermitido =
                extensao == ".nbib" ||
                extensao == ".ris";

            if (!formatoPermitido)
            {
                return BadRequest(
                    "Formato não suportado. Envie um arquivo .nbib ou .ris.");
            }

            await using var stream =
                arquivo.OpenReadStream();

            var artigos = await _service.ImportarAsync(
                revisaoId,
                stream,
                arquivo.FileName);

            return Ok(new
            {
                mensagem = "Arquivo importado com sucesso.",
                nomeArquivo = arquivo.FileName,
                formato = extensao,
                quantidadeArtigos = artigos.Count
            });
        }

        [HttpGet]
        public async Task<IActionResult> ListarArquivos(
            int revisaoId)
        {
            var importacoes = await _service
                .ListarArquivosAsync(revisaoId);

            return Ok(importacoes);
        }

        [HttpDelete("{arquivoImportacaoId:int}")]
        public async Task<IActionResult> RemoverArquivo(
            int revisaoId,
            int arquivoImportacaoId)
        {
            await _service.RemoverAsync(
                arquivoImportacaoId);

            return Ok(new
            {
                mensagem =
                    "Arquivo e artigos relacionados removidos com sucesso."
            });
        }
    }
}