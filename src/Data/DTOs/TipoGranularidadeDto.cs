namespace EscalaApi.Data.DTOs;

public class TipoGranularidadeDto
{
    public int IdTipoGranularidade { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
}
