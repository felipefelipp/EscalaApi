namespace EscalaApi.Data.Entities;

public class Integrante
{
    public int IdIntegrante { get; private set; }
    public string Nome { get; set; }
    public List<DayOfWeek> DiasDaSemanaDisponiveis { get; set; }
    public List<int> TipoIntegrante { get; set; }

    public Integrante()
    {
    }

    public Integrante(int idIntegrante)
    {
        IdIntegrante = idIntegrante;
    }

    public Integrante(int idIntegrante, string nome)
    {
        IdIntegrante = idIntegrante;
        Nome = nome;
    }

    public Integrante(int idIntegrante, string nome, List<DayOfWeek> diasDisponiveis, List<int> tipoIntegrante)
    {
        IdIntegrante = idIntegrante;
        Nome = nome;
        DiasDaSemanaDisponiveis = diasDisponiveis ?? new List<DayOfWeek>();
        TipoIntegrante = tipoIntegrante ?? new List<int>();
    }

    public Integrante(string nome, List<DayOfWeek> diasDisponiveis, List<int> tipoIntegrante)
    {
        Nome = nome;
        DiasDaSemanaDisponiveis = diasDisponiveis ?? new List<DayOfWeek>();
        TipoIntegrante = tipoIntegrante ?? new List<int>();
    }
}
