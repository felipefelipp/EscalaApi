using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class EstrategiaAlgoritmoRepository : IEstrategiaAlgoritmoRepository
{
    public async Task<List<EstrategiaAlgoritmoDto>> ListarAsync(bool apenasAtivos = true)
    {
        using var connection = DatabaseContext.GetConnection();
        var query = apenasAtivos ? EstrategiaAlgoritmoScripts.Listar : EstrategiaAlgoritmoScripts.ListarTodos;
        var result = await connection.QueryAsync<EstrategiaAlgoritmoDto>(query);
        return result.ToList();
    }

    public async Task<EstrategiaAlgoritmoDto?> ObterPorIdAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        return await connection.QueryFirstOrDefaultAsync<EstrategiaAlgoritmoDto>(
            EstrategiaAlgoritmoScripts.ObterPorId, parameters);
    }
}
