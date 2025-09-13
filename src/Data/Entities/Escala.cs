using EscalaApi.Utils.Enums;
using EscalaApi.Utils.Extensions;
using Microsoft.OpenApi.Extensions;

namespace EscalaApi.Data.Entities;

public class Escala
{
    public DateTime Data { get; set; } // Data da escala
    public Integrante Integrante { get; set; } // Detalhes do integrante
    //[JsonIgnore]
    public int TipoEscala { get; set; }

    private DiasDaSemana DiaSemana => Data.DayOfWeek.ParaValor();
    public string DiaDaSemana => Enum.GetName(DiaSemana) ?? string.Empty;  // Dia da semana em formato string
    // Construtor
    public Escala(Integrante integrante, DateTime data, int tipoEscala)
    {
        Data = data;
        Integrante = integrante;
        TipoEscala = tipoEscala;
    }

    // Construtor vazio (opcional, necessário para serialização)
    public Escala() { }
}
