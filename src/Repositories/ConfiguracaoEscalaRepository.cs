using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class ConfiguracaoEscalaRepository : IConfiguracaoEscalaRepository
{
    public async Task<int> InserirAsync(ConfiguracaoEscalaRequest request)
    {
        using var connection = DatabaseContext.GetConnection();
        var p = new DynamicParameters();
        p.Add("@Nome", request.Nome);
        p.Add("@DataInicio", request.DataInicio.Date);
        p.Add("@DataFim", request.DataFim.Date);
        return await connection.ExecuteScalarAsync<int>(ConfiguracaoEscalaScripts.Inserir, p);
    }

    public async Task<ConfiguracaoEscala?> ObterPorIdAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var config = await connection.QueryFirstOrDefaultAsync<ConfiguracaoEscala>(
            ConfiguracaoEscalaScripts.ObterPorId, new { Id = id });
        if (config is null) return null;
        config.ValoresRecorrentes = await ObterSlotsAsync(id);
        config.TiposIntegrante = await ObterTiposAsync(id);
        return config;
    }

    public async Task<List<ConfiguracaoEscala>> ListarAsync()
    {
        using var connection = DatabaseContext.GetConnection();
        var configs = (await connection.QueryAsync<ConfiguracaoEscala>(ConfiguracaoEscalaScripts.Listar)).ToList();
        foreach (var c in configs)
        {
            c.ValoresRecorrentes = await ObterSlotsAsync(c.IdConfiguracao);
            c.TiposIntegrante = await ObterTiposAsync(c.IdConfiguracao);
        }
        return configs;
    }

    public async Task AtualizarAsync(int id, ConfiguracaoEscalaRequest request)
    {
        using var connection = DatabaseContext.GetConnection();
        await connection.ExecuteAsync(ConfiguracaoEscalaScripts.Atualizar, new
        {
            Id = id,
            Nome = request.Nome,
            DataInicio = request.DataInicio.Date,
            DataFim = request.DataFim.Date
        });
    }

    public async Task InserirSlotsAsync(int idConfiguracao, IEnumerable<int> slots)
    {
        using var connection = DatabaseContext.GetConnection();
        foreach (var slot in slots.Distinct())
            await connection.ExecuteAsync(ConfiguracaoEscalaScripts.InserirSlot,
                new { IdConfiguracao = idConfiguracao, ValorSlot = slot });
    }

    public async Task InserirTiposAsync(int idConfiguracao, IEnumerable<int> tipos)
    {
        using var connection = DatabaseContext.GetConnection();
        foreach (var tipo in tipos.Distinct())
            await connection.ExecuteAsync(ConfiguracaoEscalaScripts.InserirTipo,
                new { IdConfiguracao = idConfiguracao, IdTipo = tipo });
    }

    public async Task<List<int>> ObterSlotsAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var result = await connection.QueryAsync<int>(ConfiguracaoEscalaScripts.ObterSlots, new { Id = id });
        return result.ToList();
    }

    public async Task<List<int>> ObterTiposAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var result = await connection.QueryAsync<int>(ConfiguracaoEscalaScripts.ObterTipos, new { Id = id });
        return result.ToList();
    }

    public async Task RemoverSlotsETiposAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        await connection.ExecuteAsync(ConfiguracaoEscalaScripts.ExcluirSlots, new { Id = id });
        await connection.ExecuteAsync(ConfiguracaoEscalaScripts.ExcluirTipos, new { Id = id });
    }

    public async Task<int> ContarEscalasVinculadasAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        return await connection.ExecuteScalarAsync<int>(
            ConfiguracaoEscalaScripts.ContarEscalasVinculadas, new { Id = id });
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var connection = DatabaseContext.GetConnection();
        var rows = await connection.ExecuteAsync(
            ConfiguracaoEscalaScripts.ExcluirSoft, new { Id = id });
        return rows > 0;
    }
}
