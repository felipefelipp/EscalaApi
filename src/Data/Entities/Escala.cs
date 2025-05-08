using System.Text.Json.Serialization;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Entities;

public class Escala
{
    public DateTime Data { get; set; } // Data da escala
    public Integrante Integrante { get; set; } // Detalhes do integrante
    //[JsonIgnore]
    public TipoEscala TipoEscala { get; set; }
    // Construtor
    public Escala(Integrante integrante, DateTime data, DayOfWeek nomeSemana, TipoEscala tipoEscala)
    {
        Data = data;
        Integrante = integrante;
        TipoEscala = tipoEscala;
    }

    // Construtor vazio (opcional, necessário para serialização)
    public Escala() { }
}
