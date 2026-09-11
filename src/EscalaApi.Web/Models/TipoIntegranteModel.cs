namespace EscalaApi.Web.Models;

public class TipoIntegranteModel
{
    public int IdTipoIntegrante { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; }
}

public class TipoIntegranteRequest
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
}
