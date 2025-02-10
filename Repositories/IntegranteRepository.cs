using AutoMapper;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Repositories;

public class IntegranteRepository : IIntegranteRepository
{
    private readonly IMapper _mapper;

    public IntegranteRepository(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Integrante?> ObterIntegrantePorId(int idIntegrante)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@IdIntegrante", idIntegrante);

        await using var connection = DatabaseContext.GetConnection();

        const string queryResult = IntegranteScripts.ObterIntegrantePorIdScript;

        var result = await connection.QueryAsync<IntegranteDto>(queryResult, parameters);

        if (result.Any())
        {
            var diasDisponiveis = result.Select(r => (DayOfWeek)r.DiaDisponivel).ToList();
            var tiposIntegrante = result.Select(r => (TipoIntegrante)r.TipoIntegrante).ToList();

            var integrante = new Integrante(idIntegrante, result.First().Nome, diasDisponiveis, tiposIntegrante);
            return integrante;
        }

        return null;
    }

    public async Task<List<Integrante>?> ObterIntegrantesPorTipo(TipoIntegrante tipoIntegrante)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@TipoIntegrante", tipoIntegrante);

        await using var connection = DatabaseContext.GetConnection();

        const string queryResult = IntegranteScripts.ObterIntegrantePorTipoScript;

        // Obtenha os dados do banco como DTOs
        var result = await connection.QueryAsync<IntegranteDto>(queryResult, parameters);

        if (result.Any())
        {
            var integrantesPorId = result.GroupBy(r => r.IdIntegrante);

            var integrantes = new List<Integrante>();

            foreach (var grupo in integrantesPorId)
            {
                var idIntegrante = grupo.Key;
                var nome = grupo.First().Nome; // Pegando o nome de qualquer registro no grupo
                var diasDisponiveis = grupo.Select(g => (DayOfWeek)g.DiaDisponivel).ToList();
                var tiposIntegrante = grupo.Select(g => (TipoIntegrante)g.TipoIntegrante).Distinct().ToList();

                // Aqui consideramos apenas os integrantes que têm o tipo desejado
                if (tiposIntegrante.Contains(tipoIntegrante))
                {
                    integrantes.Add(new Integrante(idIntegrante.Value, nome, diasDisponiveis, tiposIntegrante));
                }
            }

            return integrantes.ToList();
        }

        return null;
    }

    public async Task<(IEnumerable<Integrante> integrantes, int total)> ObterIntegrantes(int pageNumber, int pageSize)
    {
        int offset = pageSize * (pageNumber - 1);
        var parameters = new
        {
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = DatabaseContext.GetConnection();

        const string query = IntegranteScripts.ObterTodosOsIntegrantes;

        using var multi = await connection.QueryMultipleAsync(query, parameters);

        var result = await multi.ReadAsync<IntegranteDto>();
        var totalCount = await multi.ReadSingleAsync<int>();

        // Obtenha os dados do banco como DTOs

        if (result.Any())
        {
            var integrantesPorId = result.GroupBy(r => r.IdIntegrante);

            var integrantes = new List<Integrante>();

            foreach (var grupo in integrantesPorId)
            {
                var idIntegrante = grupo.Key;
                var nome = grupo.First().Nome; // Pegando o nome de qualquer registro no grupo
                var diasDisponiveis = grupo.Select(g => (DayOfWeek)g.DiaDisponivel).ToList();
                var tiposIntegrante = grupo.Select(g => (TipoIntegrante)g.TipoIntegrante).Distinct().ToList();

                integrantes.Add(new Integrante(idIntegrante.Value, nome, diasDisponiveis, tiposIntegrante));
            }

            return (integrantes.ToList(), totalCount);
        }

        return (new List<Integrante>(), totalCount);
    }

    public async Task<int> InserirIntegrante(IntegranteDto integrante)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@nome", integrante.Nome);

        await using var connection = DatabaseContext.GetConnection();
        const string insertResult = IntegranteScripts.InserirIntegrante;

        var result = await connection.ExecuteScalarAsync<int>(insertResult, parameters);
        return result;
    }
}