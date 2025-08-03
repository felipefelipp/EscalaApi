namespace EscalaApi.Data.DTOs;

public class EscalaResponse
{
    public int IdEscala { get; set; }
    public DateTime? Data { get; set; }
    public string Dia
    {
        get
        {
            return Data.HasValue ? Data.Value.ToString("dddd", new System.Globalization.CultureInfo("pt-BR")) : string.Empty;
        }
    }
    public int? IdIntegrante { get; set; }
    public string? NomeIntegrante { get; set; }
    public int CodigoTipoEscala { get; set; }
    public string? DescricaoTipoEscala { get; set; }
}