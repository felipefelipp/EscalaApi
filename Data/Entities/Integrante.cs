using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Entities;

public class Integrante
{
    public int IdIntegrante { get; set; }
    public string Nome { get; set; }
    public List<DayOfWeek> DiasDisponiveis { get; set; }
    public List<TipoIntegrante> TipoIntegrante { get; set; }

    public Integrante()
    {
    }

    public Integrante(int idIntegrante, string nome, List<DayOfWeek> diasDisponiveis, List<TipoIntegrante> tipoIntegrante)
    {
        IdIntegrante = idIntegrante;
        Nome = nome;
        DiasDisponiveis = diasDisponiveis ?? new List<DayOfWeek>();
        TipoIntegrante = tipoIntegrante ?? new List<TipoIntegrante>();
    }
    
    public Integrante(string nome, List<DayOfWeek> diasDisponiveis, List<TipoIntegrante> tipoIntegrante)
    {
        Nome = nome;
        DiasDisponiveis = diasDisponiveis ?? new List<DayOfWeek>();
        TipoIntegrante = tipoIntegrante ?? new List<TipoIntegrante>();
    }
}
