namespace EscalaApi.Data.DTOs;

public class EscalaDto
{
    public int IdEscala { get; set; }
    public int? IdIntegrante { get; set; }
    public DateTime? Data { get; set; }
    public int TipoEscala { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string DescricaoTipoEscala { get; set; } = string.Empty;
    public int? IdConfiguracao { get; set; }
}
