namespace EscalaApi.Web.Models;

public class IntegranteModel
{
    public int IdIntegrante { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<DayOfWeek> DiasDaSemanaDisponiveis { get; set; } = [];
    public List<int> TipoIntegrante { get; set; } = [];
}

public class IntegranteRequest
{
    public int IdIntegrante { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<DayOfWeek> DiasDaSemanaDisponiveis { get; set; } = [];
    public List<int> TipoIntegrante { get; set; } = [];
}

public class IntegranteListResponse
{
    public bool Sucess { get; set; }
    public List<IntegranteModel> Integrantes { get; set; } = [];
    public int Total { get; set; }
    public int StatusCode { get; set; }
    public bool IsValid { get; set; }
}
