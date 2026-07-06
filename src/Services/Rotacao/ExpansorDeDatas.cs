namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Expande um intervalo de datas filtrando pelos dias da semana recorrentes.
/// </summary>
public static class ExpansorDeDatas
{
    /// <summary>
    /// Retorna todas as datas entre <paramref name="inicio"/> e <paramref name="fim"/> (inclusive)
    /// cujo dia da semana está em <paramref name="dias"/>, em ordem cronológica.
    /// </summary>
    public static List<DateTime> Expand(DateTime inicio, DateTime fim, List<DayOfWeek> dias)
    {
        if (dias.Count == 0)
            return [];

        var diasSet = dias.ToHashSet();
        var resultado = new List<DateTime>();
        var atual = inicio.Date;
        var limite = fim.Date;

        while (atual <= limite)
        {
            if (diasSet.Contains(atual.DayOfWeek))
                resultado.Add(atual);

            atual = atual.AddDays(1);
        }

        return resultado;
    }
}
