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
    public int HorasExpiracaoPreview { get; init; } = 24;
}

/// <summary>
/// Orquestra a geração determinística de escalas em modo preview.
/// </summary>
public sealed class GeradorDePreview
{
    private readonly ResolvedorEstrategia _resolvedorEstrategia;
    private readonly SeletorDeIntegrante _seletor;
    private readonly RelatorioBalanceamento _relatorioBalanceamento;
    private readonly IArmazenamentoPreview _armazenamentoPreview;

    public GeradorDePreview(
        ResolvedorEstrategia resolvedorEstrategia,
        SeletorDeIntegrante seletor,
        RelatorioBalanceamento relatorioBalanceamento,
        IArmazenamentoPreview armazenamentoPreview)
    {
        _resolvedorEstrategia = resolvedorEstrategia;
        _seletor = seletor;
        _relatorioBalanceamento = relatorioBalanceamento;
        _armazenamentoPreview = armazenamentoPreview;
    }

    public async Task<ResultadoPreview> GerarAsync(ParametrosGeracaoPreview parametros)
    {
        var estrategia = _resolvedorEstrategia.Resolver(parametros.CodigoEstrategia);
        var datas = ExpansorDeDatas.Expand(parametros.DataInicio, parametros.DataFim, parametros.DiasDaSemana);
        var historico = parametros.Historico.ToList();
        var lote = new LoteDeEscalas();

        foreach (var data in datas)
        foreach (var tipo in parametros.TiposIntegrante)
        {
            if (SlotOcupado(data, tipo, historico, lote))
                continue;

            var pool = _seletor.ObterCandidatos(
                tipo,
                data,
                parametros.Integrantes,
                lote,
                historico,
                parametros.ImpedirMultiplosTiposMesmoDia);

            if (pool.Count == 0)
            {
                lote.AdicionarWarning(data, tipo);
                continue;
            }

            var escolhido = _seletor.EscolherPorMenorCarga(pool, estrategia, tipo, data, historico, lote);
            if (escolhido is null)
            {
                lote.AdicionarWarning(data, tipo);
                continue;
            }

            lote.Adicionar(new Escala(escolhido, data.Date, tipo));
        }

        var expiraEm = DateTime.UtcNow.AddHours(parametros.HorasExpiracaoPreview);
        var token = await _armazenamentoPreview.SalvarAsync(
            lote,
            parametros.ConfiguracaoEscalaId,
            expiraEm,
            parametros.CodigoEstrategia);

        var balanceamento = _relatorioBalanceamento.Gerar(
            lote,
            historico,
            parametros.Integrantes,
            parametros.TiposIntegrante,
            estrategia,
            datas);

        return ResultadoPreview.Criar(
            lote,
            token,
            expiraEm,
            ObterInfoEstrategia(parametros.CodigoEstrategia),
            balanceamento);
    }

    public ResultadoPreview Gerar(ParametrosGeracaoPreview parametros) =>
        GerarAsync(parametros).GetAwaiter().GetResult();

    private static bool SlotOcupado(DateTime data, int tipoId, List<Escala> historico, LoteDeEscalas lote) =>
        historico.Any(e => e.Data.Date == data.Date && e.TipoEscala == tipoId) ||
        lote.JaOcupado(data, tipoId);

    private static EstrategiaUtilizadaInfo ObterInfoEstrategia(string codigo) => codigo switch
    {
        "contextual_dia_semana" => new EstrategiaUtilizadaInfo
        {
            Id = 1,
            Codigo = "contextual_dia_semana",
            Nome = "Contextual por dia da semana"
        },
        "global" => new EstrategiaUtilizadaInfo
        {
            Id = 2,
            Codigo = "global",
            Nome = "Contagem global"
        },
        _ => new EstrategiaUtilizadaInfo { Codigo = codigo, Nome = codigo }
    };
}
