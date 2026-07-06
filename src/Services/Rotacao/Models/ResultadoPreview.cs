using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao.Models;

public sealed class EstrategiaUtilizadaInfo
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}

public sealed class ResultadoPreview
{
    public string PreviewToken { get; set; } = string.Empty;
    public DateTime ExpiraEm { get; set; }
    public EstrategiaUtilizadaInfo EstrategiaUtilizada { get; set; } = new();
    public List<Escala> Escalas { get; set; } = [];
    public List<BalanceamentoItem> Balanceamento { get; set; } = [];
    public List<EscalaWarning> Warnings { get; set; } = [];

    public static ResultadoPreview Criar(
        LoteDeEscalas lote,
        string previewToken,
        DateTime expiraEm,
        EstrategiaUtilizadaInfo estrategia,
        List<BalanceamentoItem> balanceamento) =>
        new()
        {
            PreviewToken = previewToken,
            ExpiraEm = expiraEm,
            EstrategiaUtilizada = estrategia,
            Escalas = lote.Escalas.OrderBy(e => e.Data).ThenBy(e => e.TipoEscala).ToList(),
            Balanceamento = balanceamento,
            Warnings = lote.Warnings.ToList()
        };
}
