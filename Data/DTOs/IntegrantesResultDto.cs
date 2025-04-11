using EscalaApi.Data.Entities;

namespace EscalaApi.Data.DTOs;

public class IntegrantesResultDto
{
    public IEnumerable<Integrante> Integrantes { get; set; }
    public int Total { get; set; }

    public IntegrantesResultDto(IEnumerable<Integrante> integrantes, int total)
    {
        Integrantes = integrantes;
        Total = total;
    }

    public IntegrantesResultDto()
    {
            
    }
}
