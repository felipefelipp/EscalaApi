using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Define como a carga de um integrante é calculada para fins de equidade na rotação.
/// </summary>
public interface IEstrategiaContagem
{
    /// <summary>
    /// Calcula quantas vezes o integrante foi escalado no contexto da estratégia,
    /// considerando histórico persistido e o lote em geração.
    /// </summary>
    int Calcular(
        Integrante integrante,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote);
}
