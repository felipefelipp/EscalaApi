using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Request;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class TipoIntegranteCatalogoRepository : ITipoIntegranteCatalogoRepository
{
    public async Task<List<TipoIntegranteCatalogoDto>> ListarAsync(bool apenasAtivos = true)
    {
        using var connection = DatabaseContext.GetConnection();
        var query = apenasAtivos ? TipoIntegranteCatalogoScripts.Listar : TipoIntegranteCatalogoScripts.ListarTodos;
        var result = await connection.QueryAsync<TipoIntegranteCatalogoDto>(query);
        return result.ToList();
    }

    public async Task<TipoIntegranteCatalogoDto?> ObterPorIdAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        return await connection.QueryFirstOrDefaultAsync<TipoIntegranteCatalogoDto>(
            TipoIntegranteCatalogoScripts.ObterPorId, parameters);
    }

    public async Task<int> ContarAtivosAsync()
    {
        using var connection = DatabaseContext.GetConnection();
        return await connection.ExecuteScalarAsync<int>(TipoIntegranteCatalogoScripts.ContarAtivos);
    }

    public async Task<int> InserirAsync(TipoIntegranteCatalogoRequest request)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Nome", request.Nome, DbType.String);
        parameters.Add("@Descricao", request.Descricao, DbType.String);
        return await connection.ExecuteScalarAsync<int>(TipoIntegranteCatalogoScripts.Inserir, parameters);
    }

    public async Task<bool> AtualizarAsync(int id, TipoIntegranteCatalogoRequest request)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        parameters.Add("@Nome", request.Nome, DbType.String);
        parameters.Add("@Descricao", request.Descricao, DbType.String);
        return await connection.ExecuteAsync(TipoIntegranteCatalogoScripts.Atualizar, parameters) > 0;
    }

    public async Task<bool> ExcluirSoftAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        return await connection.ExecuteAsync(TipoIntegranteCatalogoScripts.ExcluirSoft, parameters) > 0;
    }

    public async Task<bool> ExisteNomeAsync(string nome, int idExcluir = 0)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Nome", nome, DbType.String);
        parameters.Add("@IdExcluir", idExcluir, DbType.Int32);
        var count = await connection.ExecuteScalarAsync<int>(TipoIntegranteCatalogoScripts.ExisteNome, parameters);
        return count > 0;
    }
}
