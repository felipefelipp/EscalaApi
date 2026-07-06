using EscalaApi.Data.Entities;
using EscalaApi.Services.Rotacao;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Tests.Rotacao;

public abstract class CenarioJoaoMariaBase
{
    protected const int TipoRecepcionista = 1;

    protected static readonly Integrante Joao = new(
        1,
        "João",
        [DayOfWeek.Wednesday, DayOfWeek.Sunday],
        [TipoRecepcionista]);

    protected static readonly Integrante Maria = new(
        2,
        "Maria",
        [DayOfWeek.Sunday],
        [TipoRecepcionista]);

    protected static ParametrosGeracaoPreview CriarParametros(string codigoEstrategia) => new()
    {
        ConfiguracaoEscalaId = 1,
        DataInicio = new DateTime(2026, 1, 7),
        DataFim = new DateTime(2026, 1, 18),
        DiasDaSemana = [DayOfWeek.Wednesday, DayOfWeek.Sunday],
        TiposIntegrante = [TipoRecepcionista],
        Integrantes = [Joao, Maria],
        Historico = [],
        CodigoEstrategia = codigoEstrategia,
        ImpedirMultiplosTiposMesmoDia = true
    };

    protected static GeradorDePreview CriarGerador()
    {
        var resolvedor = new ResolvedorEstrategia();
        var calculador = new CalculadorDeCarga();
        var seletor = new SeletorDeIntegrante(calculador, resolvedor);
        var relatorio = new RelatorioBalanceamento();
        var armazenamento = new ArmazenamentoPreviewMemoria();

        return new GeradorDePreview(resolvedor, seletor, relatorio, armazenamento);
    }

    protected static Dictionary<DateTime, int> ParaMapaAtribuicoes(ResultadoPreview resultado) =>
        resultado.Escalas.ToDictionary(e => e.Data.Date, e => e.Integrante.IdIntegrante);
}
