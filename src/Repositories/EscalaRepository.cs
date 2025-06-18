using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class EscalaRepository : IEscalaRepository
{
    public async Task<List<EscalaDto>> ObterEscalas()
    {
        using var connection = DatabaseContext.GetConnection();

        try
        {
            const string query = EscalaScripts.ObterEscala;

            var result = await connection.QueryAsync<EscalaDto>(query);

            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter escalas: {ex.Message}", ex);
        }
    }

    public async Task<EscalaDto> ObterEscalaPorId(int idEscala)
    {
        using var connection = DatabaseContext.GetConnection();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdEscala", idEscala, DbType.Int32);

            const string query = EscalaScripts.ObterEscalaPorId;

            var result = await connection.QueryAsync<EscalaDto>(query, parameters);

            return result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter escala: {ex.Message}", ex);
        }
    }
    
    // public async Task<EscalaDto> ObterEscalaPorData(DateTime dataInicio, DateTime dataFim)
    // {
    //     using var connection = DatabaseContext.GetConnection();

    //     try
    //     {
    //         var parameters = new DynamicParameters();
    //         parameters.Add("@IdEscala", idEscala, DbType.Int32);
    //         parameters.Add("@IdEscala", idEscala, DbType.Int32);

    //         const string query = EscalaScripts.ObterEscalaPorId;

    //         var result = await connection.QueryAsync<EscalaDto>(query, parameters);

    //         return result.FirstOrDefault();
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception($"Erro ao obter escala: {ex.Message}", ex);
    //     }
    // }

    public async Task InserirEscala(List<EscalaDto> escalaDto)
    {
        using var connection = DatabaseContext.GetConnection();
        try
        {
            const string query = EscalaScripts.InserirEscala;
            await connection.ExecuteAsync(query, escalaDto);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao inserir escala: {ex.Message}", ex);
        }
    }

    public async Task<bool> AtualizarEscala(int idEscala, EscalaDto escalaDto)
    {
        using var connection = DatabaseContext.GetConnection();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdIntegrante", escalaDto.IdIntegrante, DbType.Int32);
            parameters.Add("@Data", escalaDto.Data, DbType.DateTime);
            parameters.Add("@TipoEscala", escalaDto.TipoEscala, DbType.Int32);
            parameters.Add("@IdEscala", idEscala, DbType.Int32);

            const string query = EscalaScripts.AtualizarEscala;
            int linhasAfetadas = await connection.ExecuteAsync(query, parameters);

            return linhasAfetadas > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar escala: {ex.Message}", ex);
        }
    }
}