using System.Text.Json.Serialization;

namespace EscalaApi.Data.DTOs;

public class IntegranteDto
{
    public int? IdIntegrante { get; set; }
    public string? Nome { get; set; }
    public List<int> DiasDaSemanaDisponiveis { get; set; } 
    public List<int> TipoIntegrante { get; set; } 
}   