using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao.Estrategias;

/// <summary>
/// Conta escalas por (tipo, dia da semana) dentro da mesma configuração.
/// </summary>
public sealed class ContextualPorDiaSemana : IEstrategiaContagem
{
    public int Calcular(
        Integrante integrante,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote)
    {
        var contexto = ContextoRotacao.ParaData(tipoId, data);

        return lote.TodasAsEscalas(historico).Count(e =>
            contexto.CorrespondeIntegrante(e, integrante));
    }

    public ContextoRotacao ObterContexto(int tipoId, DateTime data) =>
        ContextoRotacao.ParaData(tipoId, data);
}
