using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepaMed.Application.Services;
using UepaMed.Domain.Enums;

namespace UepaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/revisoes/{revisaoId}/importacoes")]
    public class ArquivosController : ControllerBase
    {
        private readonly ImportacaoArtigosService _service;

        public ArquivosController(ImportacaoArtigosService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> ImportarArquivos(
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
            });
        }
        [HttpGet]
        public async Task<IActionResult> ListarArquivos(int revisaoId)
        {
            var importacoes = await _service.ListarArquivosAsync(revisaoId);

            return Ok(importacoes);
        }

        //[HttpGet("artigos")]
        //public async Task<IActionResult> ListarArtigos(int revisaoId)
        //{
        //    var importacoes = await _service.ListarArtigosAsync(revisaoId);

        //    return Ok(importacoes);
        //}

        //[HttpPut("{artigoId}/status")]
        //public async Task<IActionResult> MudarStatusArtigo(
        //int artigoId,
        //[FromBody] StatusArtigo status)
        //{
        //    await _service.MudarStatusArtigo(artigoId, status);

        //    return NoContent();
        //}

        [HttpDelete("{arquivoImportacaoId}")]
        public async Task<IActionResult> RemoverArquivos(
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