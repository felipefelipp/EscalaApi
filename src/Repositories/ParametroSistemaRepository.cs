using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class ParametroSistemaRepository : IParametroSistemaRepository
{
    public async Task<List<ParametroSistemaDto>> ListarAsync()
    {
        using var connection = DatabaseContext.GetConnection();
        var result = await connection.QueryAsync<ParametroSistemaDto>(ParametroSistemaScripts.Listar);
        return result.ToList();
    }

    public async Task<ParametroSistemaDto?> ObterPorChaveAsync(string chave)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Chave", chave, DbType.String);
        return await connection.QueryFirstOrDefaultAsync<ParametroSistemaDto>(
            ParametroSistemaScripts.ObterPorChave, parameters);
    }

    public async Task<bool> AtualizarValorAsync(string chave, string valor)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Chave", chave, DbType.String);
        parameters.Add("@Valor", valor, DbType.String);
        return await connection.ExecuteAsync(ParametroSistemaScripts.AtualizarPorChave, parameters) > 0;
    }

    public async Task<int> InserirAsync(string chave, string valor, string? descricao)
    {
        using var connection = DatabaseContext.GetConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Chave", chave, DbType.String);
        parameters.Add("@Valor", valor, DbType.String);
        parameters.Add("@Descricao", descricao, DbType.String);
        return await connection.ExecuteScalarAsync<int>(ParametroSistemaScripts.Inserir, parameters);
    }
}
