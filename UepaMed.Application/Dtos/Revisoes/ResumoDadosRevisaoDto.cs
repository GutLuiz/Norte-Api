namespace UepaMed.Application.Dtos.Revisoes
{
    public class ResumoDadosRevisaoDto
    {
        public int ArquivosImportados { get; set; }

        public int ArtigosDuplicados { get; set; }

        public int ArtigosPendentes { get; set; }

        public int ArtigosIncluidos { get; set; }

        public int ArtigosExcluidos { get; set; }

        public int ConflitosIdentificados { get; set; }
    }
}