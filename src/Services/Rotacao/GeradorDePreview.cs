using EscalaApi.Data.Entities;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Parâmetros completos para geração de preview sem depender de repositório de configuração.
/// </summary>
public sealed class ParametrosGeracaoPreview
{
    public int ConfiguracaoEscalaId { get; init; }
    public DateTime DataInicio { get; init; }
    public DateTime DataFim { get; init; }
    public List<DayOfWeek> DiasDaSemana { get; init; } = [];
    public List<int> TiposIntegrante { get; init; } = [];
    public List<Integrante> Integrantes { get; init; } = [];
    public IEnumerable<Escala> Historico { get; init; } = [];
    public string CodigoEstrategia { get; init; } = "contextual_dia_semana";
    public bool ImpedirMultiplosTiposMesmoDia { get; init; } = true;
}

/// <summary>
/// Orquestra a geração determinística de escalas em modo preview.
/// </summary>
public sealed class GeradorDePreview
{
    private readonly ResolvedorEstrategia _resolvedorEstrategia;
    private readonly SeletorDeIntegrante _seletor;
    private readonly RelatorioBalanceamento _relatorioBalanceamento;

    public GeradorDePreview(
        ResolvedorEstrategia resolvedorEstrategia,
        SeletorDeIntegrante seletor,
        RelatorioBalanceamento relatorioBalanceamento)
    {
        _resolvedorEstrategia = resolvedorEstrategia;
        _seletor = seletor;
        _relatorioBalanceamento = relatorioBalanceamento;
    }

    public Task<ResultadoPreview> GerarAsync(ParametrosGeracaoPreview parametros) =>
        Task.FromResult(Gerar(parametros));

    public ResultadoPreview Gerar(ParametrosGeracaoPreview parametros)
    {
        var estrategia = _resolvedorEstrategia.Resolver(parametros.CodigoEstrategia);
        var datas = ExpansorDeDatas.Expand(parametros.DataInicio, parametros.DataFim, parametros.DiasDaSemana);
        var historico = parametros.Historico.ToList();

        var lote = GerarLote(parametros, estrategia, datas, historico);
        return MontarResultado(parametros, estrategia, datas, historico, lote);
    }

    private LoteDeEscalas GerarLote(
        ParametrosGeracaoPreview parametros,
        IEstrategiaContagem estrategia,
        List<DateTime> datas,
        List<Escala> historico)
    {
        var lote = new LoteDeEscalas();

        foreach (var data in datas)
        foreach (var tipo in parametros.TiposIntegrante)
            PreencherSlot(parametros, estrategia, data, tipo, historico, lote);

        return lote;
    }

    private void PreencherSlot(
        ParametrosGeracaoPreview parametros,
        IEstrategiaContagem estrategia,
        DateTime data,
        int tipo,
        List<Escala> historico,
        LoteDeEscalas lote)
    {
        if (SlotOcupado(data, tipo, historico, lote))
            return;

        var candidatos = _seletor.ObterCandidatos(
            tipo,
            data,
            parametros.Integrantes,
            lote,
            historico,
            parametros.ImpedirMultiplosTiposMesmoDia);

        var escolhido = _seletor.EscolherPorMenorCarga(
            candidatos, estrategia, tipo, data, historico, lote);

        if (escolhido is null)
        {
            lote.AdicionarWarning(data, tipo);
            return;
        }

        lote.Adicionar(new Escala(escolhido, data.Date, tipo));
    }

    private ResultadoPreview MontarResultado(
        ParametrosGeracaoPreview parametros,
        IEstrategiaContagem estrategia,
        List<DateTime> datas,
        List<Escala> historico,
        LoteDeEscalas lote)
    {
        var balanceamento = _relatorioBalanceamento.Gerar(
            lote,
            historico,
            parametros.Integrantes,
            parametros.TiposIntegrante,
            estrategia,
            datas);

        return ResultadoPreview.Criar(lote, balanceamento);
    }

    private static bool SlotOcupado(DateTime data, int tipoId, List<Escala> historico, LoteDeEscalas lote) =>
        historico.Any(e => e.Data.Date == data.Date && e.TipoEscala == tipoId) ||
        lote.JaOcupado(data, tipoId);
}
