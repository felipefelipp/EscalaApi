using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class EscalaRepository : IEscalaRepository
{
    public async Task<List<EscalaDto>> ObterEscalas(EscalaFiltro escalaFiltro)
    {
        using var connection = DatabaseContext.GetConnection();

        try
        {
            string query = EscalaScripts.ObterEscala;
            var parametros = new DynamicParameters();
            var where = new List<string>();

            if (escalaFiltro.DataInicio.HasValue)
            {
                where.Add("Escalas.dt_data_escala >= @DataInicio");
                parametros.Add("@DataInicio", escalaFiltro.DataInicio.Value, DbType.DateTime);
            }
            if (escalaFiltro.DataFim.HasValue)
            {
                where.Add("Escalas.dt_data_escala <= @DataFim");
                parametros.Add("@DataFim", escalaFiltro.DataFim.Value, DbType.DateTime);
            }
            if (escalaFiltro.Tipo.HasValue)
            {
                where.Add("Escalas.cd_tipo_escala = @Tipo");
                parametros.Add("@Tipo", escalaFiltro.Tipo.Value, DbType.Int32);
            }

            if (where.Count > 0)
                query += " WHERE " + string.Join(" AND ", where);

            query += " ORDER BY Escalas.dt_data_escala ASC";

            if (escalaFiltro.Skip > 0 || escalaFiltro.Take > 0)
            {
                query += " OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
            }

            parametros.Add("@Skip", escalaFiltro.Skip);
            parametros.Add("@Take", escalaFiltro.Take);

            var result = await connection.QueryAsync<EscalaDto>(query, parametros);

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