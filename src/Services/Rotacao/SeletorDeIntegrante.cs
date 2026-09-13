using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Obtém candidatos elegíveis e aplica desempate determinístico por menor carga.
/// </summary>
public sealed class SeletorDeIntegrante
{
    public List<Integrante> ObterCandidatos(
        int tipoId,
        DateTime data,
        IEnumerable<Integrante> integrantes,
        LoteDeEscalas lote,
        IEnumerable<Escala> historico,
        bool impedirMultiplosTiposMesmoDia,
        bool evitarConsecutivosMesmaFuncao = true)
    {
        var candidatos = integrantes
            .Where(i => i.TipoIntegrante.Contains(tipoId))
            .Where(i => i.DiasDaSemanaDisponiveis.Contains(data.DayOfWeek))
            .Where(i => !impedirMultiplosTiposMesmoDia || !JaAtribuidoOutroTipoMesmoDia(i, data, tipoId, historico, lote))
            .ToList();

        if (evitarConsecutivosMesmaFuncao && candidatos.Count > 1)
        {
            var todasEscalas = lote.TodasAsEscalas(historico);
            var ultimaEscala = todasEscalas
                .Where(e => e.TipoEscala == tipoId && e.Data.Date < data.Date)
                .OrderByDescending(e => e.Data)
                .FirstOrDefault();

            if (ultimaEscala != null)
            {
                var alternativos = candidatos
                    .Where(c => c.IdIntegrante != ultimaEscala.Integrante.IdIntegrante)
                    .ToList();

                if (alternativos.Count > 0)
                    return alternativos;
            }
        }

        return candidatos;
    }

    public Integrante? EscolherPorMenorCarga(
        IEnumerable<Integrante> candidatos,
        IEstrategiaContagem estrategia,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote,
        bool desempateAleatorio = false)
    {
        var pool = candidatos.ToList();
        if (pool.Count == 0)
            return null;

        var contagens = pool
            .Select(i => (
                Integrante: i,
                Contagem: estrategia.Calcular(i, tipoId, data, historico, lote)))
            .ToList();

        var minContagem = contagens.Min(x => x.Contagem);
        var empatados = contagens
            .Where(x => x.Contagem == minContagem)
            .Select(x => x.Integrante)
            .ToList();

        return Desempatar(empatados, estrategia, tipoId, data, historico, lote, desempateAleatorio);
    }

    private static Integrante Desempatar(
        List<Integrante> empatados,
        IEstrategiaContagem estrategia,
        int tipoId,
        DateTime data,
        IEnumerable<Escala> historico,
        LoteDeEscalas lote,
        bool desempateAleatorio)
    {
        if (empatados.Count <= 1)
            return empatados.First();

        var contexto = estrategia.ObterContexto(tipoId, data);
        var todasEscalas = lote.TodasAsEscalas(historico).ToList();

        // 1. Prioriza quem tem a data da última escala mais antiga (ou nunca escalado)
        var comDataMaisAntiga = empatados
            .GroupBy(i => ObterDataUltimaEscalaNoContexto(i, contexto, todasEscalas))
            .OrderBy(g => g.Key)
            .First()
            .ToList();

        if (comDataMaisAntiga.Count == 1)
            return comDataMaisAntiga[0];

        // 2. Se desempateAleatorio estiver habilitado (modo dinâmico/web), sorteia entre os empatados
        if (desempateAleatorio)
            return comDataMaisAntiga[Random.Shared.Next(comDataMaisAntiga.Count)];

        // 3. Fallback determinístico (PRD §10.6): menor id_integrante para testes unitários
        return comDataMaisAntiga.OrderBy(i => i.IdIntegrante).First();
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
