namespace EscalaApi.Web.Models;

public class ConfiguracaoEscalaModel
{
    public int IdConfiguracao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; } = DateTime.Today;
    public DateTime DataFim { get; set; } = DateTime.Today.AddMonths(1);
    public bool Ativo { get; set; } = true;
    public List<int> ValoresRecorrentes { get; set; } = [];
    public List<int> TiposIntegrante { get; set; } = [];
}

public class ConfiguracaoEscalaRequest
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; } = DateTime.Today;
    public DateTime DataFim { get; set; } = DateTime.Today.AddMonths(1);
    public List<int> ValoresRecorrentes { get; set; } = [];
    public List<int> TiposIntegrante { get; set; } = [];
}
