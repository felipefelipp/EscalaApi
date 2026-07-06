namespace EscalaApi.Tests.Rotacao;

/// <summary>
/// Cenário João/Maria com estratégia de contagem global (PRD §10.7).
/// </summary>
public class CenarioJoaoMariaEstrategia2Tests : CenarioJoaoMariaBase
{
    [Fact]
    public void DeveFavorecerMariaNosDomingos()
    {
        var gerador = CriarGerador();
        var resultado = gerador.Gerar(CriarParametros("global"));
        var atribuicoes = ParaMapaAtribuicoes(resultado);

        Assert.Equal(4, resultado.Escalas.Count);
        Assert.Equal(1, atribuicoes[new DateTime(2026, 1, 7)]);   // Qua → João
        Assert.Equal(2, atribuicoes[new DateTime(2026, 1, 11)]);  // Dom → Maria (João=1, Maria=0)
        Assert.Equal(1, atribuicoes[new DateTime(2026, 1, 14)]);  // Qua → João
        Assert.Equal(2, atribuicoes[new DateTime(2026, 1, 18)]);  // Dom → Maria (João=3, Maria=1)
    }

    [Fact]
    public void DeveUsarEstrategiaGlobalNoResultado()
    {
        var gerador = CriarGerador();
        var resultado = gerador.Gerar(CriarParametros("global"));

        Assert.Equal("global", resultado.EstrategiaUtilizada.Codigo);
        Assert.Equal(2, resultado.EstrategiaUtilizada.Id);
    }

    [Fact]
    public void RelatorioGlobal_NaoDeveAgruparPorDiaSemana()
    {
        var gerador = CriarGerador();
        var resultado = gerador.Gerar(CriarParametros("global"));

        var itemGlobal = resultado.Balanceamento
            .Single(b => b.TipoIntegranteId == TipoRecepcionista && b.DiaSemana is null);

        Assert.Contains(itemGlobal.Contagens, c => c.IntegranteId == 1 && c.Total == 2);
        Assert.Contains(itemGlobal.Contagens, c => c.IntegranteId == 2 && c.Total == 2);
    }
}
