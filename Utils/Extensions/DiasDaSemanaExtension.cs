using EscalaApi.Utils.Enums;

namespace EscalaApi.Utils.Extensions;

public static class DiasDaSemanaExtension
{
    public static DiasDaSemana ParaValor(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Sunday => DiasDaSemana.Domingo,
            DayOfWeek.Monday => DiasDaSemana.Segunda,
            DayOfWeek.Tuesday => DiasDaSemana.Terca,
            DayOfWeek.Wednesday => DiasDaSemana.Quarta,
            DayOfWeek.Thursday => DiasDaSemana.Quinta,
            DayOfWeek.Friday => DiasDaSemana.Sexta,
            DayOfWeek.Saturday => DiasDaSemana.Sabado,
            _ => default,
        };
    }

}