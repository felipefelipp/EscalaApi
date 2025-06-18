using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class TipoEscalaRepository : ITipoEscalaRepository
{
    public async Task<List<int>> ObterTiposEscalaDisponiveis()
    {
        using var connection = DatabaseContext.GetConnection();

        try
        {
            var parameters = new DynamicParameters();

            const string query = TipoEscalaScripts.ObterTiposEscalaDisponiveis;

            var result = await connection.QueryAsync<int>(query, parameters);

            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter escala: {ex.Message}", ex);
        }
    }
}