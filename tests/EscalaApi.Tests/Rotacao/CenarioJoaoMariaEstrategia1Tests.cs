namespace EscalaApi.Tests.Rotacao;

/// <summary>
/// Cenário João/Maria com estratégia contextual por dia da semana (PRD §10.7).
/// </summary>
public class CenarioJoaoMariaEstrategia1Tests : CenarioJoaoMariaBase
{
    [Fact]
    public void DeveGerarAtribuicoesContextuaisDeterministicas()
    {
        var gerador = CriarGerador();
        var resultado = gerador.Gerar(CriarParametros("contextual_dia_semana"));
        var atribuicoes = ParaMapaAtribuicoes(resultado);

        Assert.Equal(4, resultado.Escalas.Count);
        Assert.Equal(1, atribuicoes[new DateTime(2026, 1, 7)]);   // Qua → João
        Assert.Equal(1, atribuicoes[new DateTime(2026, 1, 11)]);  // Dom → João (empate inicial, id menor)
        Assert.Equal(1, atribuicoes[new DateTime(2026, 1, 14)]);  // Qua → João
        Assert.Equal(2, atribuicoes[new DateTime(2026, 1, 18)]);  // Dom → Maria (menor contagem contextual)
    }

    [Fact]
    public void DeveManterContagemContextualPorDomingo()
    {
        var gerador = CriarGerador();
        var resultado = gerador.Gerar(CriarParametros("contextual_dia_semana"));

        var domingo = resultado.Balanceamento
            .Single(b => b.DiaSemana == "Domingo" && b.TipoIntegranteId == TipoRecepcionista);

        Assert.Equal(1, domingo.Contagens.Single(c => c.IntegranteId == 1).Total);
        Assert.Equal(1, domingo.Contagens.Single(c => c.IntegranteId == 2).Total);
        Assert.Equal(0, domingo.DesvioMaximo);
    }

    [Fact]
    public void DeveSerDeterministicoEmExecucoesRepetidas()
    {
        var gerador = CriarGerador();
        var parametros = CriarParametros("contextual_dia_semana");

        var primeira = gerador.Gerar(parametros);
        var segunda = gerador.Gerar(parametros);

        Assert.Equal(
            primeira.Escalas.Select(e => (e.Data, e.Integrante.IdIntegrante)),
            segunda.Escalas.Select(e => (e.Data, e.Integrante.IdIntegrante)));
    }
}
