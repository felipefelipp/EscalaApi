namespace EscalaApi.Services;



public static class ValidadorRangeMaximo

{

    public const string ChaveParametro = "range_maximo_escala";

    public const string ValorPadrao = "mensal";



    private static readonly HashSet<string> ValoresValidos = new(StringComparer.OrdinalIgnoreCase)

    {

        "semanal", "mensal", "trimestral", "semestral", "anual", "ilimitado"

    };



    private static readonly Dictionary<string, int?> DiasMaximosPorCodigo = new(StringComparer.OrdinalIgnoreCase)

    {

        ["semanal"] = 7,

        ["mensal"] = 31,

        ["trimestral"] = 92,

        ["semestral"] = 184,

        ["anual"] = 365,

        ["ilimitado"] = null

    };



    public static bool EhValido(string? valor) =>

        !string.IsNullOrWhiteSpace(valor) && ValoresValidos.Contains(valor.Trim());



    public static string Normalizar(string valor) => valor.Trim().ToLowerInvariant();



    public static string ResolverValorOuPadrao(string? valor) =>

        string.IsNullOrWhiteSpace(valor) ? ValorPadrao : Normalizar(valor);



    public static int? ObterDiasMaximos(string codigo) =>

        DiasMaximosPorCodigo.TryGetValue(Normalizar(codigo), out var dias) ? dias : null;



    public static IReadOnlyCollection<string> ValoresAceitos => ValoresValidos;

    public static bool RangeValido(DateTime inicio, DateTime fim, string codigo, out int diasPermitidos)
    {
        var max = ObterDiasMaximos(ResolverValorOuPadrao(codigo));
        var dias = (fim.Date - inicio.Date).Days + 1;
        diasPermitidos = max ?? int.MaxValue;
        return max is null || dias <= max;
    }
}

