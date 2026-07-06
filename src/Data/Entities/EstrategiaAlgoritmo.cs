namespace EscalaApi.Data.Entities;

public class EstrategiaAlgoritmo
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string DescricaoDetalhada { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
