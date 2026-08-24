
using UepaMed.Application.Interfaces;
using UepaMed.Domain.Entities;
using UepaMed.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UepaMed.Infrastructure.Repositories
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
