namespace EscalaApi.Data.DTOs;

public record EscalaDto(int? IdIntegrante, DateTime? Data, int TipoEscala)
{
    public EscalaDto() : this(null, null, 0) { }
    public EscalaDto(string Nome): this(null, null, 0) { }
};