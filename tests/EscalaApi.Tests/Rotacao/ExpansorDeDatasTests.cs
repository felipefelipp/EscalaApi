using EscalaApi.Services.Rotacao;

namespace EscalaApi.Tests.Rotacao;

public class ExpansorDeDatasTests
{
    [Fact]
    public void Expand_DeveRetornarDatasNoIntervaloComDiasCorretos()
    {
        var inicio = new DateTime(2026, 1, 7);
        var fim = new DateTime(2026, 1, 18);
        var dias = new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Sunday };

        var resultado = ExpansorDeDatas.Expand(inicio, fim, dias);

        Assert.Equal(4, resultado.Count);
        Assert.Equal(new DateTime(2026, 1, 7), resultado[0]);
        Assert.Equal(new DateTime(2026, 1, 11), resultado[1]);
        Assert.Equal(new DateTime(2026, 1, 14), resultado[2]);
        Assert.Equal(new DateTime(2026, 1, 18), resultado[3]);
    }

    [Fact]
    public void Expand_ComListaVazia_DeveRetornarVazio()
    {
        var resultado = ExpansorDeDatas.Expand(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 1, 31),
            []);

        Assert.Empty(resultado);
    }

    [Fact]
    public void Expand_DeveIncluirLimitesDoIntervalo()
    {
        var quarta = new DateTime(2026, 1, 7);
        var resultado = ExpansorDeDatas.Expand(quarta, quarta, [DayOfWeek.Wednesday]);

        Assert.Single(resultado);
        Assert.Equal(quarta, resultado[0]);
    }
}
