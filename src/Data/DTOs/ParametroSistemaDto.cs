namespace EscalaApi.Data.DTOs;

public class ParametroSistemaDto
{
    public int IdParametro { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
