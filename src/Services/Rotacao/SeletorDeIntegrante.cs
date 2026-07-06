using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Obtém candidatos elegíveis e aplica desempate determinístico por menor carga.
/// </summary>
public sealed class SeletorDeIntegrante
{
    private readonly CalculadorDeCarga _calculadorDeCarga;
    private readonly ResolvedorEstrategia _resolvedorEstrategia;

    public SeletorDeIntegrante(CalculadorDeCarga calculadorDeCarga, ResolvedorEstrategia resolvedorEstrategia)
    {
        _calculadorDeCarga = calculadorDeCarga;
        _resolvedorEstrategia = resolvedorEstrategia;
    }

    public List<Integrante> ObterCandidatos(
        int tipoId,
        DateTime data,
        IEnumerable<Integrante> integrantes,
        LoteDeEscalas lote,
        IEnumerable<Escala> historico,
        bool impedirMultiplosTiposMesmoDia)
    {
        return integrantes
            .Where(i => i.TipoIntegrante.Contains(tipoId))
            .Where(i => i.DiasDaSemanaDisponiveis.Contains(data.DayOfWeek))
            .Where(i => !impedirMultiplosTiposMesmoDia || !JaAtribuidoOutroTipoMesmoDia(i, data, tipoId, historico, lote))
            .ToList();
    }

    public Integrante? EscolherPorMenorCarga(
        IEnumerable<Integrante> candidatos,
        IEstrategiaContagem estrategia,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote)
    {
        var pool = candidatos.ToList();
        if (pool.Count == 0)
            return null;

        var contagens = pool
            .Select(i => (
                Integrante: i,
                Contagem: _calculadorDeCarga.Calcular(estrategia, i, tipoId, data, historico, lote)))
            .ToList();

        var minContagem = contagens.Min(x => x.Contagem);
        var empatados = contagens
            .Where(x => x.Contagem == minContagem)
            .Select(x => x.Integrante)
            .ToList();

        return Desempatar(empatados, estrategia, tipoId, data, historico, lote);
    }

    private Integrante Desempatar(
        List<Integrante> empatados,
        IEstrategiaContagem estrategia,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote)
    {
        var contexto = _resolvedorEstrategia.ObterContextoDesempate(estrategia, tipoId, data);
        var todasEscalas = lote.TodasAsEscalas(historico).ToList();

        return empatados
            .OrderBy(i => ObterDataUltimaEscalaNoContexto(i, contexto, todasEscalas))
            .ThenBy(i => i.IdIntegrante)
            .First();
    }

    private static DateTime ObterDataUltimaEscalaNoContexto(
        Integrante integrante,
        ContextoRotacao contexto,
        List<Escala> escalas)
    {
        var ultima = escalas
            .Where(e => contexto.CorrespondeIntegrante(e, integrante))
            .Select(e => e.Data.Date)
            .DefaultIfEmpty(DateTime.MinValue)
            .Max();

        return ultima;
    }

    private static bool JaAtribuidoOutroTipoMesmoDia(
        Integrante integrante,
        DateTime data,
        int tipoId,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote)
    {
        return lote.TodasAsEscalas(historico).Any(e =>
            e.Data.Date == data.Date &&
            e.Integrante.IdIntegrante == integrante.IdIntegrante &&
            e.TipoEscala != tipoId);
    }
}
