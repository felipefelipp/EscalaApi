using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Mappers;

public static class IntegranteMapper
{
    public static IntegranteDto ParaDto(this Integrante integrante)
    {
        var diasDisponiveisDto = new List<int>();
        var tipoIntegranteDto = new List<int>();

        foreach (var diaDisponivel in integrante.DiasDaSemanaDisponiveis)
        {
            diasDisponiveisDto.Add((int)diaDisponivel);
        }

        foreach (var tipo in integrante.TipoIntegrante)
        {
            tipoIntegranteDto.Add((int)tipo);
        }

        return new IntegranteDto
        {
            IdIntegrante = integrante.IdIntegrante,
            Nome = integrante.Nome,
            DiasDaSemanaDisponiveis = diasDisponiveisDto,
            TipoIntegrante = tipoIntegranteDto
        };
    }
    
    public static IntegranteDto ParaDto(this IntegranteRequest integrante)
    {
        var diasDisponiveisDto = new List<int>();
        var tipoIntegranteDto = new List<int>();

        foreach (var diaDisponivel in integrante.DiasDaSemanaDisponiveis)
        {
            diasDisponiveisDto.Add((int)diaDisponivel);
        }

        foreach (var tipo in integrante.TipoIntegrante)
        {
            tipoIntegranteDto.Add((int)tipo);
        }

        return new IntegranteDto
        {
            Nome = integrante.Nome,
            DiasDaSemanaDisponiveis = diasDisponiveisDto,
            TipoIntegrante = tipoIntegranteDto
        };
    }
    
    public static Integrante ParaIntegrante(this IntegranteDto integrante)
    {
        var diasDisponiveisDto = new List<DayOfWeek>();
        var tipoIntegranteDto = new List<TipoIntegrante>();

        foreach (var diaDisponivel in integrante.DiasDaSemanaDisponiveis)
        {
            diasDisponiveisDto.Add((DayOfWeek)diaDisponivel);
        }

        foreach (var tipo in integrante.TipoIntegrante)
        {
            tipoIntegranteDto.Add((TipoIntegrante)tipo);
        }

        return new Integrante
        {
            IdIntegrante = integrante.IdIntegrante.Value,
            Nome = integrante.Nome,
            DiasDaSemanaDisponiveis = diasDisponiveisDto,
            TipoIntegrante = tipoIntegranteDto
        };
    }
    
    public static Integrante ParaIntegrante(this IntegranteRequest integrante)
    {
        var diasDisponiveisDto = new List<DayOfWeek>();
        var tipoIntegranteDto = new List<TipoIntegrante>();

        foreach (var diaDisponivel in integrante.DiasDaSemanaDisponiveis)
        {
            diasDisponiveisDto.Add((DayOfWeek)diaDisponivel);
        }

        foreach (var tipo in integrante.TipoIntegrante)
        {
            tipoIntegranteDto.Add((TipoIntegrante)tipo);
        }

        return new Integrante
        {
            Nome = integrante.Nome,
            DiasDaSemanaDisponiveis = diasDisponiveisDto,
            TipoIntegrante = tipoIntegranteDto
        };
    }
}