namespace EscalaApi.Services.Rotacao.Models;

public sealed class BalanceamentoItem
{
    public int TipoIntegranteId { get; set; }
    public string? TipoIntegranteNome { get; set; }
    public string? DiaSemana { get; set; }
    public List<ContagemIntegrante> Contagens { get; set; } = [];
    public int DesvioMaximo { get; set; }
}
