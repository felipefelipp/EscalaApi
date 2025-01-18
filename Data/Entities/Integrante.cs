using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Entities;

public record Integrante(
    int IdIntegrante,
    string Nome,
    List<DayOfWeek> DiasDisponiveis,
    List<TipoIntegrante> TipoIntegrante
)
{
    public Integrante(int idIntegrante) : this(idIntegrante, null, null, null) { }
};
