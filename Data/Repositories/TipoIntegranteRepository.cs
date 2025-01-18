using AutoMapper;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;

namespace Escala.Business.Data.Repository;

public class TipoIntegranteRepository
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
}