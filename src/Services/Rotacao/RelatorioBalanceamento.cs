using EscalaApi.Data.Entities;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Monta o relatório de balanceamento pós-geração com contagens e desvio máximo por contexto.
/// </summary>
public sealed class RelatorioBalanceamento
{
    public List<BalanceamentoItem> Gerar(
        LoteDeEscalas lote,
        IEnumerable<Escala> historico,
        IEnumerable<Integrante> integrantes,
        List<int> tiposIntegrante,
        IEstrategiaContagem estrategia,
        List<DateTime> datas)
    {
        var todasEscalas = lote.TodasAsEscalas(historico).ToList();
        var integrantesLista = integrantes.ToList();
        var itens = new List<BalanceamentoItem>();
        var contextos = ObterContextosUnicos(estrategia, tiposIntegrante, datas);

        foreach (var contexto in contextos)
        {
            itens.Add(CriarItem(contexto, todasEscalas, integrantesLista));
        }

        return itens;
    }

    private static IEnumerable<ContextoRotacao> ObterContextosUnicos(
        IEstrategiaContagem estrategia,
        List<int> tiposIntegrante,
        List<DateTime> datas)
    {
        return tiposIntegrante
            .SelectMany(tipoId => datas.Select(data => estrategia.ObterContexto(tipoId, data)))
            .Distinct();
    }

    private static BalanceamentoItem CriarItem(
        ContextoRotacao contexto,
        List<Escala> todasEscalas,
        List<Integrante> integrantes)
    {
        var contagens = integrantes
            .Where(i => i.TipoIntegrante.Contains(contexto.TipoId))
            .Select(i => new ContagemIntegrante
            {
                IntegranteId = i.IdIntegrante,
                Nome = i.Nome,
                Total = todasEscalas.Count(e => contexto.CorrespondeIntegrante(e, i))
            })
            .Where(c => c.Total > 0)
            .OrderBy(c => c.IntegranteId)
            .ToList();

        var valores = contagens.Select(c => c.Total).ToList();
        var desvio = valores.Count > 0 ? valores.Max() - valores.Min() : 0;

        return new BalanceamentoItem
        {
            TipoIntegranteId = contexto.TipoId,
            DiaSemana = contexto.DiaSemana.HasValue ? ObterNomeDia(contexto.DiaSemana.Value) : null,
            Contagens = contagens,
            DesvioMaximo = desvio
        };
    }

    private static string ObterNomeDia(DayOfWeek dia) => dia switch
    {
        DayOfWeek.Sunday => "Domingo",
        DayOfWeek.Monday => "Segunda",
        DayOfWeek.Tuesday => "Terça",
        DayOfWeek.Wednesday => "Quarta",
        DayOfWeek.Thursday => "Quinta",
        DayOfWeek.Friday => "Sexta",
        DayOfWeek.Saturday => "Sábado",
        _ => dia.ToString()
    };
}
