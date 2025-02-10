using AutoMapper;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class TipoIntegranteRepository : ITipoIntegranteRepository
{
    private readonly IMapper _mapper;

    public TipoIntegranteRepository(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IEnumerable<Integrante> ObterTiposIntegrante(int idIntegrante)
    {
        using var connection = DatabaseContext.GetConnection();

        var sql = @"SELECT tipo_integrante
                    FROM Tipo_integrante
                    WHERE id_integrante = @idIntegrante";

        var result = connection.Query<TipoIntegranteDto>(sql, new { idIntegrante }).ToList();

        return _mapper.Map<IEnumerable<Integrante>>(result);
    }

    public async Task<bool> InserirTipoIntegrante(List<TipoIntegranteDto> tipoIntegranteDto)
    {
        try
        {
            const string insertResult = TipoIntegranteScripts.InserirTipoIntegrante;

            await using var connection = DatabaseContext.GetConnection();

            await connection.ExecuteAsync(insertResult, tipoIntegranteDto);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}