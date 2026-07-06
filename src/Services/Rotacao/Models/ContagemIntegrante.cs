namespace EscalaApi.Services.Rotacao.Models;

public sealed class ContagemIntegrante
{
    public int IntegranteId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Total { get; set; }
}
