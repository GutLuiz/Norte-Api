using UepaMed.Domain.Enums.Votacao;

namespace UepaMed.Application.Dtos.Votacoes
{
    public class RegistrarVotoDto
    {
        public int ArtigoId { get; set; }

        public int UsuarioId { get; set; }

        public OpcaoVoto Opcao { get; set; }
    }
}