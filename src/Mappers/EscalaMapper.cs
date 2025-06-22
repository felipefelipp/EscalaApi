using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;

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
                Integrante = new(
                    escala.IdIntegrante.Value,
                    escala.Nome
                ),
                Data = escala.Data.Value,
                TipoEscala = escala.TipoEscala
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
            Integrante = new(
                escalaDto.IdIntegrante.Value,
                escalaDto.Nome
            ),
            Data = escalaDto.Data.Value,
            TipoEscala = escalaDto.TipoEscala
        };
        return escala;
    }

    public static List<EscalaResultDto> ParaListaEscalaDto(this List<EscalaDto> escalasDto)
    {
        List<EscalaResultDto> escalas = new();
        foreach (var escala in escalasDto)
        {
            escalas.Add(new EscalaResultDto
            {
                IdEscala = escala.IdEscala,
                Data = escala.Data.Value,
                IdIntegrante = escala.IdIntegrante.Value,
                NomeIntegrante = escala.Nome,
                CodigoTipoEscala = escala.TipoEscala,
                DescricaoTipoEscala = escala.DescricaoTipoEscala
            });
        }
        return escalas;
    }
}