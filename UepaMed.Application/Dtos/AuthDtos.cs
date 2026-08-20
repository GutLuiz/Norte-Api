namespace UepaMed.Application.DTOs
{
    public record RegistroDto(string Nome, string Email, string Senha);
    public record LoginDto(string Email, string Senha);
    public record RefreshDto(string RefreshToken);
    public record TokenResponseDto(string AccessToken, string RefreshToken);
}
