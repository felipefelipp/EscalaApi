namespace EscalaApi.Data.DTOs;

public class TipoIntegranteCatalogoDto
{
    public int IdTipoIntegrante { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
