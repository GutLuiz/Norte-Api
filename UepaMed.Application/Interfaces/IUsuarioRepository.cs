using UepaMed.Domain.Entities;

namespace UepaMed.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorEmailAsync(string email);
    }
}
