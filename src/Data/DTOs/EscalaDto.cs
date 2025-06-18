namespace EscalaApi.Data.DTOs;

public class EscalaDto
{
    public int? IdIntegrante { get; set; }
    public DateTime? Data { get; set; }
    public int TipoEscala { get; set; }
    public string Nome { get; set; }
    public string DescricaoTipoEscala { get; set; }
}