using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class TipoGranularidadeRepository : ITipoGranularidadeRepository
{
    public async Task<List<TipoGranularidadeDto>> ListarAsync(bool apenasAtivos = true)
    {
        using var connection = DatabaseContext.GetConnection();
        var query = apenasAtivos ? TipoGranularidadeScripts.Listar : TipoGranularidadeScripts.ListarTodos;
        var result = await connection.QueryAsync<TipoGranularidadeDto>(query);
        return result.ToList();
    }

    public async Task<TipoGranularidadeDto?> ObterPorIdAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        return await connection.QueryFirstOrDefaultAsync<TipoGranularidadeDto>(
            TipoGranularidadeScripts.ObterPorId, parameters);
    }
}
