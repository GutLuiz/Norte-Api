using UepaMed.Domain.Entities.Usuarios;

namespace UepaMed.Application.Interfaces.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorEmailAsync(string email);
    }
}
