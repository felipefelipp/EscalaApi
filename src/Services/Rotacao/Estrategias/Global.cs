using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao.Estrategias;

/// <summary>
/// Conta escalas globalmente por tipo, independente do dia da semana.
/// </summary>
public sealed class Global : IEstrategiaContagem
{
    public int Calcular(
        Integrante integrante,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote)
    {
        var contexto = ObterContexto(tipoId, data);

        return lote.TodasAsEscalas(historico).Count(e =>
            contexto.CorrespondeIntegrante(e, integrante));
    }

    public ContextoRotacao ObterContexto(int tipoId, DateTime data) =>
        ContextoRotacao.PorTipo(tipoId);
}
