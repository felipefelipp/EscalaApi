namespace EscalaApi.Data.Request;

public class ConfiguracaoEscalaRequest
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int IdEstrategiaAlgoritmo { get; set; }
    public int? IdTipoGranularidade { get; set; }
    public List<int> ValoresRecorrentes { get; set; } = [];
    public List<int> TiposIntegrante { get; set; } = [];
}
