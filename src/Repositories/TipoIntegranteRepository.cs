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
    public async Task<bool> InserirTipoIntegrante(List<IntegranteDto> tipoIntegranteDto)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            const string insertResult = TipoIntegranteScripts.InserirTipoIntegrante;
            foreach (var tipo in tipoIntegranteDto)
            {
                DynamicParameters parametrosInsercao = new DynamicParameters();
                parametrosInsercao.Add("@IdIntegrante", tipoIntegranteDto.First().IdIntegrante, DbType.Int32);
                parametrosInsercao.Add("@TipoIntegrante", tipo.TipoIntegrante, DbType.Int32);
                await connection.ExecuteAsync(insertResult, parametrosInsercao);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> AtualizarTipoIntegrante(List<IntegranteDto> tipoIntegrante)
    {
        try
        {
            using var connection = DatabaseContext.GetConnection();
            DynamicParameters parametrosRemocao = new DynamicParameters();
            parametrosRemocao.Add("@IdIntegrante", tipoIntegrante.First().IdIntegrante, DbType.Int32);

            const string removeResult = TipoIntegranteScripts.ExcluirTipoIntegrante;
            await connection.ExecuteAsync(removeResult, parametrosRemocao);

            const string insertResult = TipoIntegranteScripts.InserirTipoIntegrante;

            foreach (var tipo in tipoIntegrante)
            {
                DynamicParameters parametrosInsercao = new DynamicParameters();
                parametrosInsercao.Add("@IdIntegrante", tipoIntegrante.First().IdIntegrante, DbType.Int32);
                parametrosInsercao.Add("@TipoIntegrante", tipo.TipoIntegrante, DbType.Int32);
                await connection.ExecuteAsync(insertResult, parametrosInsercao);
            }

            return true;
        }
        catch (Exception)
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
        catch (Exception)
        {
            return false;
        }
    }
}