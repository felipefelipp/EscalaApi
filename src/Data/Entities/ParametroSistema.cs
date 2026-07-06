namespace EscalaApi.Data.Entities;

public class ParametroSistema
{
    public int Id { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
