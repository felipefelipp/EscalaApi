namespace EscalaApi.Services.Rotacao.Models;

public sealed class EscalaWarning
{
    public DateTime Data { get; set; }
    public int TipoIntegranteId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
