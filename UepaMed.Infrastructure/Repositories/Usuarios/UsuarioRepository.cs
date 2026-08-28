using UepaMed.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using UepaMed.Application.Interfaces.Usuarios;
using UepaMed.Domain.Entities.Usuarios;

namespace UepaMed.Infrastructure.Repositories.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
