using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Mappers;

public static class EscalaMapper
{
    public static List<EscalaDto> ParaListaEscalaDto(this List<Escala> escalas)
    {
        List<EscalaDto> escalasDto = new();
        foreach (var escala in escalas)
        {
            escalasDto.Add(new EscalaDto
            {
                IdIntegrante = escala.Integrante.IdIntegrante,
                Data = escala.Data,
                TipoEscala = (int)escala.TipoEscala,
            });
        }

        return escalasDto;
    }

    public static List<Escala> ParaListaEscala(this List<EscalaDto> escalasDto)
    {
        List<Escala> escalas = new();
        foreach (var escala in escalasDto)
        {
            escalas.Add(new Escala
            {
                Integrante = new()
                {
                    IdIntegrante = escala.IdIntegrante.Value,
                    Nome = escala.Nome
                },
                Data = escala.Data.Value,
                TipoEscala = (TipoEscala)escala.TipoEscala
            });
        }

        return escalas;
    }

    public static EscalaDto ParaEscalaDto(this Escala escala)
    {
        EscalaDto escalaDto =
            new EscalaDto
            {
                IdIntegrante = escala.Integrante.IdIntegrante,
                Data = escala.Data,
                TipoEscala = (int)escala.TipoEscala,
            };
        
        return escalaDto;
    }

    public static Escala ParaEscala(this EscalaDto escalaDto)
    {
        Escala escala = new Escala
        {
            Integrante = new()
            {
                IdIntegrante = escalaDto.IdIntegrante.Value,
                Nome = escalaDto.Nome
            },
            Data = escalaDto.Data.Value,
            TipoEscala = (TipoEscala)escalaDto.TipoEscala
        };
        return escala;
    }
}