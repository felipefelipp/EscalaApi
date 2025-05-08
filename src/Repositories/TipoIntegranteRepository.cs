using System.Data;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class TipoIntegranteRepository : ITipoIntegranteRepository
{
    public TipoIntegranteRepository()
    {
    }
    public async Task<bool> InserirTipoIntegrante(IntegranteDto tipoIntegranteDto)
    {
        try
        {
        using var connection = DatabaseContext.GetConnection();
            const string insertResult = TipoIntegranteScripts.InserirTipoIntegrante;
            foreach (var tipo in tipoIntegranteDto.TipoIntegrante)
            {
                DynamicParameters parametrosInsercao = new DynamicParameters();
                parametrosInsercao.Add("@IdIntegrante", tipoIntegranteDto.IdIntegrante, DbType.Int32);
                parametrosInsercao.Add("@TipoIntegrante", tipo, DbType.Int32);
                await connection.ExecuteAsync(insertResult, parametrosInsercao);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> AtualizarTipoIntegrante(IntegranteDto tipoIntegrante)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            DynamicParameters parametrosRemocao = new DynamicParameters();
            parametrosRemocao.Add("@IdIntegrante", tipoIntegrante.IdIntegrante, DbType.Int32);

            const string removeResult = TipoIntegranteScripts.ExcluirTipoIntegrante;
            await connection.ExecuteAsync(removeResult, parametrosRemocao);

            const string insertResult = TipoIntegranteScripts.InserirTipoIntegrante;
            foreach (var tipo in tipoIntegrante.TipoIntegrante)
            {
                DynamicParameters parametrosInsercao = new DynamicParameters();
                parametrosInsercao.Add("@IdIntegrante", tipoIntegrante.IdIntegrante, DbType.Int32);
                parametrosInsercao.Add("@TipoIntegrante", tipo, DbType.Int32);
                await connection.ExecuteAsync(insertResult, parametrosInsercao);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> RemoverTipoIntegrante(int idIntegrante)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            DynamicParameters parametrosRemocao = new DynamicParameters();
            parametrosRemocao.Add("@IdIntegrante", idIntegrante, DbType.Int32);

            const string removeResult = TipoIntegranteScripts.ExcluirTipoIntegrante;
            await connection.ExecuteAsync(removeResult, parametrosRemocao);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}