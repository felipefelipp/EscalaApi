using EscalaApi.Services;



namespace EscalaApi.Tests;



public class ValidadorRangeMaximoTests

{

    [Theory]

    [InlineData("semanal")]

    [InlineData("mensal")]

    [InlineData("trimestral")]

    [InlineData("semestral")]

    [InlineData("anual")]

    [InlineData("ilimitado")]

    [InlineData("MENSAL")]

    [InlineData(" Trimestral ")]

    public void EhValido_DeveAceitarValoresPermitidos(string valor)

    {

        Assert.True(ValidadorRangeMaximo.EhValido(valor));

    }



    [Theory]

    [InlineData(null)]

    [InlineData("")]

    [InlineData("   ")]

    [InlineData("quinzenal")]

    [InlineData("diario")]

    public void EhValido_DeveRejeitarValoresInvalidos(string? valor)

    {

        Assert.False(ValidadorRangeMaximo.EhValido(valor));

    }



    [Theory]

    [InlineData(null, "mensal")]

    [InlineData("", "mensal")]

    [InlineData("   ", "mensal")]

    [InlineData("trimestral", "trimestral")]
    public void ResolverValorOuPadrao_DeveRetornarMensalQuandoVazio(string? valor, string esperado)
    {

        Assert.Equal(esperado, ValidadorRangeMaximo.ResolverValorOuPadrao(valor));

    }



    [Theory]

    [InlineData("semanal", 7)]

    [InlineData("mensal", 31)]

    [InlineData("trimestral", 92)]

    [InlineData("semestral", 184)]

    [InlineData("anual", 365)]

    [InlineData("ilimitado", null)]

    public void ObterDiasMaximos_DeveRetornarLimitesCorretos(string codigo, int? diasEsperados)

    {

        Assert.Equal(diasEsperados, ValidadorRangeMaximo.ObterDiasMaximos(codigo));

    }

}

