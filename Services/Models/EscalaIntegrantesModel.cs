using EscalaApi.Data.Entities;

namespace EscalaApi.Services.Models;

public class EscalaIntegrantesModel(DateTime dataInicio, DateTime dataFim, List<DayOfWeek> dias, List<Integrante> integrantes)
{
    public DateTime DataInicio = dataInicio;
    public DateTime DataFim = dataFim;
    public List<DayOfWeek> Dias = dias;
    public List<Integrante> Integrantes = integrantes;
}
