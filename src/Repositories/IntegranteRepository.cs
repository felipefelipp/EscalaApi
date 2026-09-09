using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class IntegranteRepository : IIntegranteRepository
{
    public IntegranteRepository()
    {
    }

    public async Task<List<IntegranteDto>?> ObterIntegrantePorId(int idIntegrante)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdIntegrante", idIntegrante, DbType.Int32);

            using var connection = DatabaseContext.GetConnection();

            const string queryResult = IntegranteScripts.ObterIntegrantePorIdScript;

            var result = await connection.QueryAsync<IntegranteDto>(queryResult, parameters);
            return result?.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter integrante por ID: {ex.Message}", ex);
        }
    }

    public async Task<List<IntegranteDto>?> ObterIntegrantesPorTipo(int tipoIntegrante)
    {
        try
        {
            DynamicParameters parameters = new();
            parameters.Add("@TipoIntegrante", tipoIntegrante);

            using var connection = DatabaseContext.GetConnection();

            const string queryResult = IntegranteScripts.ObterIntegrantesPorTipoScript;

            var result = await connection.QueryAsync<IntegranteDto>(queryResult, parameters);

            return result?.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter integrantes por tipo: {ex.Message}", ex);
        }
    }

    public async Task<(List<IntegranteDto>, int)> ObterIntegrantes(IntegranteFiltro filtro)
    {
        using var connection = DatabaseContext.GetConnection();

        try
        {
            var where = new List<string>();
            var parameters = new DynamicParameters();

            if (filtro.TipoIntegrante is > 0)
            {
                where.Add("integrante_tipo.cd_tipo_integrante = @TipoIntegrante");
                parameters.Add("@TipoIntegrante", filtro.TipoIntegrante, DbType.Int32);
            }

            if (filtro.DiaDisponivel.HasValue)
            {
                where.Add("integrantes_dias_disponiveis.cd_dia_disponivel = @DiaDisponivel");
                parameters.Add("@DiaDisponivel", (int)filtro.DiaDisponivel.Value, DbType.Int32);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                where.Add("integrantes.desc_nome = @Nome");
                parameters.Add("@Nome", filtro.Nome, DbType.String);
            }

            var whereClause = where.Count > 0
                ? " WHERE " + string.Join(" AND ", where)
                : string.Empty;

            var fromJoins = IntegranteScripts.FromIntegrantesComJoins;

            var countQuery = $@"
                SELECT COUNT(DISTINCT integrantes.id_integrante)
                {fromJoins}
                {whereClause}";

            var total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            parameters.Add("@Skip", filtro.Skip, DbType.Int32);
            parameters.Add("@Take", filtro.Take, DbType.Int32);

            // Pagina por integrante (ID distinto), depois carrega dias e tipos completos.
            var idsCte = filtro.Take > 0
                ? $@"
                ;WITH PagedIds AS (
                    SELECT DISTINCT integrantes.id_integrante
                    {fromJoins}
                    {whereClause}
                    ORDER BY integrantes.id_integrante
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                )"
                : $@"
                ;WITH PagedIds AS (
                    SELECT DISTINCT integrantes.id_integrante
                    {fromJoins}
                    {whereClause}
                )";

            var query = idsCte + string.Format(
                IntegranteScripts.SelecionarIntegrantesCompletosPorIds,
                "PagedIds");

            var integrantes = await connection.QueryAsync<IntegranteDto>(query, parameters);

            if (integrantes == null || !integrantes.Any())
                return ([], total);

            return (integrantes.ToList(), total);
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
