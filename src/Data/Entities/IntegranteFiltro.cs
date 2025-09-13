namespace EscalaApi.Data.Entities;

public class IntegranteFiltro : Filtro
{
    public int? TipoIntegrante { get; set; }
    public DayOfWeek? DiaDisponivel { get; set; } 
    public string? Nome { get; set; }
}