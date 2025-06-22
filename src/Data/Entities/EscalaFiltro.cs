namespace EscalaApi.Data.Entities;

public class EscalaFiltro
{
    public int Skip { get; set; }
    public int Take { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int? Tipo { get; set; }
}