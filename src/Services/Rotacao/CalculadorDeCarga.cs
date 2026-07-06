using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Delega o cálculo de carga para a estratégia de contagem selecionada.
/// </summary>
public sealed class CalculadorDeCarga
{
    public int Calcular(
        IEstrategiaContagem estrategia,
        Integrante integrante,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote) =>
        estrategia.Calcular(integrante, tipoId, data, historico, lote);
}
