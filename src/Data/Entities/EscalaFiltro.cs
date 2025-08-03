namespace EscalaApi.Data.Entities;

public class EscalaFiltro : Filtro
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int? Tipo { get; set; }
}