namespace EscalaApi.Data.Entities;

public class ConfiguracaoEscala
{
    public int IdConfiguracao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int IdEstrategiaAlgoritmo { get; set; }
    public string CodigoEstrategia { get; set; } = string.Empty;
    public int IdTipoGranularidade { get; set; } = 1;
    public bool EstrategiaImutavel { get; set; }
    public bool Ativo { get; set; } = true;
    public List<int> ValoresRecorrentes { get; set; } = [];
    public List<int> TiposIntegrante { get; set; } = [];
}
