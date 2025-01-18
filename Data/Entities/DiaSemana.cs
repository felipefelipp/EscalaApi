namespace EscalaApi.Data.Entities;

public class DiaSemana(DateTime data, DayOfWeek dayOfWeek)
{
    public DateTime Data = data;
    public DayOfWeek DayOfWeek = dayOfWeek;
}