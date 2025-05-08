using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Repositories;

public class EscalaRepository : IEscalaRepository
{
    public async Task<List<Data.Entities.Escala>> ObterEscalas()
    {
        var escala = new List<Data.Entities.Escala>();

        using var connection = DatabaseContext.GetConnection();

        const string queryResult = EscalaScripts.ObterEscala;

        var result = await connection.QueryAsync<EscalaDto>(queryResult);

        foreach (var escalaDto in result)
        {
            escala.Add(new Data.Entities.Escala()
            {
                Data = escalaDto.Data.Value,
                TipoEscala = (TipoEscala)escalaDto.TipoEscala,
                Integrante = new Integrante(escalaDto.IdIntegrante.Value)
            });
        }

        return escala;
    }

    public async Task InserirEscala(List<EscalaDto> escalaDto)
    {
        const string insertResult = EscalaScripts.InserirEscala;

        using var connection = DatabaseContext.GetConnection();

        await connection.ExecuteAsync(insertResult, escalaDto);
    }
}