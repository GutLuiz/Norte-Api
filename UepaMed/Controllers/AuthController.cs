using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UepaMed.Infrastructure.Data;
using UepaMed.Domain.Entities;
using UepaMed.Application.Services;
using UepaMed.Application.Dtos.Usuario;
using UepaMed.Domain.Entities.Usuarios;

namespace UepaMed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext db, TokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroDto dto)
        {
            if (await _db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("E-mail já cadastrado.");
            }
                

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            return Ok("Usuário criado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {
                return Unauthorized("Credenciais inválidas.");
            }
                

            var accessToken = _tokenService.GerarAccessToken(usuario);
            var refreshToken = _tokenService.GerarRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiraEm = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return Ok(new TokenResponseDto(accessToken, refreshToken));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponseDto>> Refresh(RefreshDto dto)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);

            if (usuario is null || usuario.RefreshTokenExpiraEm < DateTime.UtcNow)
            {
                return Unauthorized("Refresh token inválido ou expirado.");
            }
            var novoAccessToken = _tokenService.GerarAccessToken(usuario);
            var novoRefreshToken = _tokenService.GerarRefreshToken();

            usuario.RefreshToken = novoRefreshToken;
            usuario.RefreshTokenExpiraEm = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return Ok(new TokenResponseDto(novoAccessToken, novoRefreshToken));
        }
    }
}
