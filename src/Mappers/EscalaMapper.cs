using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;

namespace EscalaApi.Mappers;

public static class EscalaMapper
{
    public static List<EscalaDto> ParaListaDto(this List<Escala> escalas)
    {
        List<EscalaDto> escalasDto = new();
        foreach (var escala in escalas)
        {
            escalasDto.Add(new EscalaDto {
                IdIntegrante = escala.Integrante.IdIntegrante,
                Data = escala.Data,
                TipoEscala = (int) escala.TipoEscala,
            });
        }

        return escalasDto;
    }
}