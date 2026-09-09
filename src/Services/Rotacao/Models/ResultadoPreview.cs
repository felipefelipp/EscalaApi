using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao.Models;

public sealed class ResultadoPreview
{
    public List<Escala> Escalas { get; set; } = [];
    public List<BalanceamentoItem> Balanceamento { get; set; } = [];
    public List<EscalaWarning> Warnings { get; set; } = [];

    public static ResultadoPreview Criar(
        LoteDeEscalas lote,
        List<BalanceamentoItem> balanceamento) =>
        new()
        {
            Escalas = lote.Escalas.OrderBy(e => e.Data).ThenBy(e => e.TipoEscala).ToList(),
            Balanceamento = balanceamento,
            Warnings = lote.Warnings.ToList()
        };
}
