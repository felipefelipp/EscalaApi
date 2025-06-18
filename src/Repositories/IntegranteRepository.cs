using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Utils.Enums;
using Exception = System.Exception;

namespace EscalaApi.Repositories;

public class IntegranteRepository : IIntegranteRepository
{
    public IntegranteRepository()
    {
    }

    public async Task<Integrante?> ObterIntegrantePorId(int idIntegrante)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdIntegrante", idIntegrante, DbType.Int32);

            using var connection = DatabaseContext.GetConnection();

            const string queryResult = IntegranteScripts.ObterIntegrantePorIdScript;

            var result = await connection.QueryAsync(queryResult, parameters);

            if (result.Any())
            {
                var diasDisponiveis = result.Where(r => r.DiaDisponivel != null).Select(r => (int)r.DiaDisponivel)
                    .Distinct().ToList();
                var tiposIntegrante = result.Where(r => r.TipoIntegrante != null).Select(r => (int)r.TipoIntegrante)
                    .Distinct().ToList();

                var integrante = new IntegranteDto
                {
                    IdIntegrante = idIntegrante,
                    Nome = result.First().Nome,
                    DiasDaSemanaDisponiveis = diasDisponiveis,
                    TipoIntegrante = tiposIntegrante,
                };
                return integrante.ParaIntegrante();
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter integrante por ID: {ex.Message}", ex);
        }
    }

    public async Task<List<Integrante>?> ObterIntegrantesPorTipo(int tipoIntegrante)
    {
        try
        {
            DynamicParameters parameters = new();
            parameters.Add("@TipoIntegrante", tipoIntegrante);

            using var connection = DatabaseContext.GetConnection();

            const string queryResult = IntegranteScripts.ObterIntegrantePorTipoScript;

            var result = await connection.QueryAsync(queryResult, parameters);

            if (result.Any())
            {
                var integrantesPorId = result.GroupBy(r => r.IdIntegrante);
                var integrantes = new List<Integrante>();

                foreach (var grupo in integrantesPorId)
                {
                    var idIntegrante = grupo.Key;
                    var nome = grupo.First().Nome;
                    var diasDisponiveis = grupo.Where(r => r.DiaDisponivel != null).Select(g => (int)g.DiaDisponivel)
                        .ToList();
                    var tiposIntegrante = grupo.Where(r => r.TipoIntegrante != null).Select(g => (int)g.TipoIntegrante)
                        .Distinct().ToList();

                    if (tiposIntegrante.Contains((int)tipoIntegrante))
                    {
                        var integrante = new IntegranteDto
                        {
                            IdIntegrante = (int)idIntegrante,
                            Nome = (string)nome,
                            DiasDaSemanaDisponiveis = diasDisponiveis,
                            TipoIntegrante = tiposIntegrante,
                        };
                        integrantes.Add(integrante.ParaIntegrante());
                    }
                }

                return integrantes.ToList();
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter integrantes por tipo: {ex.Message}", ex);
        }
    }

    public async Task<IntegrantesResultDto> ObterIntegrantes(int pageNumber, int pageSize)
    {
        try
        {
            int offset = pageSize * (pageNumber - 1);
            var parameters = new
            {
                PageSize = pageSize,
                Offset = offset
            };

            using var connection = DatabaseContext.GetConnection();

            const string query = IntegranteScripts.ObterTodosOsIntegrantes;

            using var multi = await connection.QueryMultipleAsync(query, parameters);

            var result = await multi.ReadAsync<dynamic>();
            var totalCount = await multi.ReadSingleAsync<int>();

            if (result.Any())
            {
                var integrantesPorId = result.GroupBy(r => (int)r.IdIntegrante);
                var integrantes = new List<Integrante>();

                foreach (var grupo in integrantesPorId)
                {
                    var firstRecord = grupo.First();
                    var integrante = new IntegranteDto
                    {
                        IdIntegrante = (int)firstRecord.IdIntegrante,
                        Nome = (string)firstRecord.Nome,
                        DiasDaSemanaDisponiveis = grupo
                            .Where(r => r.DiaDisponivel != null)
                            .Select(g => (int)g.DiaDisponivel)
                            .ToList(),
                        TipoIntegrante = grupo
                            .Where(r => r.TipoIntegrante != null)
                            .Select(g => (int)g.TipoIntegrante)
                            .Distinct()
                            .ToList(),
                    };

                    integrantes.Add(integrante.ParaIntegrante());
                }

                return new IntegrantesResultDto(integrantes, totalCount);
            }

            return new IntegrantesResultDto(new List<Integrante>(), 0);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter integrantes: {ex.Message}", ex);
        }
    }

    public async Task<bool> AtualizarIntegrante(IntegranteDto integrante)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@nome", integrante.Nome, DbType.String);
            parameters.Add("@idIntegrante", integrante.IdIntegrante, DbType.Int32);

            using var connection = DatabaseContext.GetConnection();
            const string updateResult = IntegranteScripts.AtualizarIntegrante;

            await connection.ExecuteAsync(updateResult, parameters);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar integrante: {ex.Message}", ex);
        }
    }

    public async Task<int> InserirIntegrante(IntegranteDto integrante)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@nome", integrante.Nome, DbType.String);

            using var connection = DatabaseContext.GetConnection();
            const string insertResult = IntegranteScripts.InserirIntegrante;

            var result = await connection.ExecuteScalarAsync<int>(insertResult, parameters);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao inserir integrante: {ex.Message}", ex);
        }
    }

    public async Task<bool> RemoverIntegrante(int idIntegrante)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@idIntegrante", idIntegrante, DbType.Int32);
            const string removeResult = IntegranteScripts.RemoverIntegrante;

            await connection.ExecuteAsync(removeResult, parameters);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar integrante: {ex.Message}", ex);
        }
    }
}