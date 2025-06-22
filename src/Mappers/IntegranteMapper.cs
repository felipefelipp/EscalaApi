using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;

namespace EscalaApi.Mappers;

public static class IntegranteMapper
{
    public static Integrante ParaIntegrante(this List<IntegranteDto> integrantesDto)
    {
        return integrantesDto
            .GroupBy(i => i.IdIntegrante)
            .Select(grupo =>
                new Integrante(grupo.Key ?? 0,
                    grupo.First().Nome ?? "",
                    grupo
                        .Select(i => (DayOfWeek)i.DiaDaSemanaDisponivel)
                        .Distinct()
                        .ToList(),
                    grupo
                        .Select(i => i.TipoIntegrante)
                        .Distinct()
                        .ToList())
            ).FirstOrDefault() ?? new Integrante();
    }

    public static List<Integrante> ParaIntegrantes(this List<IntegranteDto> integrantesDto)
    {
        var integrantes = integrantesDto
            .GroupBy(i => i.IdIntegrante)
            .Select(grupo =>
                new Integrante(
                    grupo.Key ?? 0,
                    grupo.First().Nome ?? "",
                    grupo
                        .Select(i => (DayOfWeek)i.DiaDaSemanaDisponivel)
                        .Distinct()
                        .ToList(),
                    grupo
                        .Select(i => i.TipoIntegrante)
                        .Distinct()
                        .ToList()
            )).ToList();

        return integrantes;
    }

    public static List<IntegranteDto> ParaDtos(this Integrante integrante)
    {
        var dtos = new List<IntegranteDto>();

        foreach (var dia in integrante.DiasDaSemanaDisponiveis)
        {
            foreach (var tipo in integrante.TipoIntegrante)
            {
                dtos.Add(new IntegranteDto
                {
                    IdIntegrante = integrante.IdIntegrante,
                    Nome = integrante.Nome,
                    DiaDaSemanaDisponivel = (int)dia,
                    TipoIntegrante = tipo
                });
            }
        }

        return dtos;
    }

    public static List<IntegranteDto> ParaDtos(this List<Integrante> integrantes)
    {
        var dtos = new List<IntegranteDto>();

        foreach (var integrante in integrantes)
        {
            dtos.AddRange(integrante.ParaDtos());
        }

        return dtos;
    }

    public static Integrante ParaIntegrante(this IntegranteRequest integrante, int idIntegrante)
    {
        var diasDisponiveis = new List<DayOfWeek>();
        var tipoIntegrante = new List<int>();

        foreach (var diaDisponivel in integrante.DiasDaSemanaDisponiveis)
        {
            diasDisponiveis.Add(diaDisponivel);
        }

        foreach (var tipo in integrante.TipoIntegrante)
        {
            tipoIntegrante.Add(tipo);
        }

        return new Integrante(idIntegrante,
                              integrante.Nome,
                              diasDisponiveis,
                              tipoIntegrante);
    }
}