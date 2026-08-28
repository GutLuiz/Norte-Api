public class ConviteListaDto
{
    public int Id { get; set; }
    public int RevisaoId { get; set; }
    public string TituloRevisao { get; set; } = string.Empty;
    public string NomeProprietario { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}