using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Request;

public class IntegranteRequest
{
    public string Nome { get; set; }

    public List<DayOfWeek> DiasDaSemanaDisponiveis { get; set; }

    public List<TipoIntegrante> TipoIntegrante { get; set; }
}