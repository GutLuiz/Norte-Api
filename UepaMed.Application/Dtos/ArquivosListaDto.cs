using UepaMed.Domain.Enums;

namespace UepaMed.Application.Dtos
{
    public class ArquivosListaDto
    {
        public int Id { get; set; }
        public string NomeArquivo { get; set; }
        public int QuantidadeArtigos { get; set; }
        public TipoArquivoImportacao TipoArquivo { get; set; }
    }
}
