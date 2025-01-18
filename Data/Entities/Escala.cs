namespace EscalaApi.Data.Entities;

public class Escala(Integrante integrante, DateTime data, DayOfWeek nomeSemana)
{
    public DateTime Data = data;
    public DayOfWeek NomeSemana = nomeSemana;
    public Integrante Integrante = integrante;
}