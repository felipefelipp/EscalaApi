using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Contexto de comparação para contagem e desempate: tipo de integrante e, opcionalmente, dia da semana.
/// </summary>
public sealed record ContextoRotacao(int TipoId, DayOfWeek? DiaSemana)
{
    public static ContextoRotacao PorTipo(int tipoId) => new(tipoId, null);

    public static ContextoRotacao PorTipoEDia(int tipoId, DayOfWeek diaSemana) =>
        new(tipoId, diaSemana);

    public static ContextoRotacao ParaData(int tipoId, DateTime data) =>
        PorTipoEDia(tipoId, data.DayOfWeek);

    public bool Corresponde(Escala escala) =>
        escala.TipoEscala == TipoId &&
        (DiaSemana is null || escala.Data.DayOfWeek == DiaSemana);

    public bool CorrespondeIntegrante(Escala escala, Integrante integrante) =>
        escala.Integrante.IdIntegrante == integrante.IdIntegrante && Corresponde(escala);
}
