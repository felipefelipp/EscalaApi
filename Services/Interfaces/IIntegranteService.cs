using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Services.Results;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Services.Interfaces;

public interface IIntegranteService
{
    Task<Result<Integrante>> ObterIntegrantePorId(int idIntegrante);
    Task<Result<List<Integrante>>> ObterIntegrantesPorTipo(TipoIntegrante tipoIntegrante);
    Task<Result<Integrante>> RegistrarIntegrante(IntegranteRequest integranteRequest);
}