using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class IntegranteDiasDisponiveisRepository : IIntegranteDiasDisponiveisRepository
{
    public async Task<bool> InserirDiasDisponiveis(List<IntegranteDto> diasDisponiveisDto)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            const string insertResult = IntegranteDiasDisponiveisScripts.InserirIntegranteDiasDisponiveis;
            DynamicParameters parametrosInsercao = new DynamicParameters();

            foreach (var diaDisponivel in diasDisponiveisDto)
            {
                parametrosInsercao.Add("@IdIntegrante", diasDisponiveisDto.First().IdIntegrante, DbType.Int32);
                parametrosInsercao.Add("@DiaDisponivel", diaDisponivel.DiaDaSemanaDisponivel, DbType.Int32);
                await connection.ExecuteAsync(insertResult, parametrosInsercao);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> AtualizarDiasDisponiveis(List<IntegranteDto> diasDisponiveisDto)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdIntegrante", diasDisponiveisDto.First().IdIntegrante, DbType.Int32);

            const string removeResult = IntegranteDiasDisponiveisScripts.RemoverIntegranteDiasDisponiveis;
            await connection.ExecuteAsync(removeResult, parameters);

            const string insertResult = IntegranteDiasDisponiveisScripts.InserirIntegranteDiasDisponiveis;
            DynamicParameters parametrosInsercao = new DynamicParameters();

            foreach (var diaDisponivel in diasDisponiveisDto)
            {
                parametrosInsercao.Add("@IdIntegrante", diasDisponiveisDto.First().IdIntegrante, DbType.Int32);
                parametrosInsercao.Add("@DiaDisponivel", diaDisponivel.DiaDaSemanaDisponivel, DbType.Int32);
                await connection.ExecuteAsync(insertResult, parametrosInsercao);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemoverDiasDisponiveis(int idIntegrante)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdIntegrante", idIntegrante, DbType.Int32);

            const string removeResult = IntegranteDiasDisponiveisScripts.RemoverIntegranteDiasDisponiveis;
            await connection.ExecuteAsync(removeResult, parameters);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}