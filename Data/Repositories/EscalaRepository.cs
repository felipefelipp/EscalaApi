using Dapper;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Repositories.Interfaces;
using EscalaApi.Data.Scripts;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Repositories;

public class EscalaRepository : IEscalaRepository
{
    public async Task<List<Entities.Escala>> ObterEscalas()
    {
        var escala = new List<Entities.Escala>();

        await using var connection = DatabaseContext.GetConnection();

        const string queryResult = EscalaScripts.ObterEscala;
        
        var result = await connection.QueryAsync<EscalaDto>(queryResult);

        foreach (var escalaDto in result)
        {
            escala.Add(new Entities.Escala()
            {
                Data = escalaDto.Data.Value,
                TipoEscala = (TipoEscala) escalaDto.TipoEscala,
                Integrante = new Integrante(escalaDto.IdIntegrante.Value)
            });
        }

        return escala;
    }

    public async Task InserirEscala(List<Entities.Escala> escalas)
    {
        var escalaDto = new List<EscalaDto>();
        foreach (var escala in escalas)
        {
            escalaDto.Add(new EscalaDto(escala.Integrante.IdIntegrante, escala.Data, (int)escala.TipoEscala));
        }

        const string insertResult = EscalaScripts.InserirEscala;

        await using var connection = DatabaseContext.GetConnection();

        await connection.ExecuteAsync(insertResult, escalaDto);
    }
}