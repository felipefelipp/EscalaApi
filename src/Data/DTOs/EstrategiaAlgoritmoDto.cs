namespace EscalaApi.Data.DTOs;

public class EstrategiaAlgoritmoDto
{
    public int IdEstrategiaAlgoritmo { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string DescricaoDetalhada { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
