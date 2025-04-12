using EscalaApi.Data.DTOs;
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
    Task<Result<IntegrantesResultDto>> ObterIntegrantes(int pageNumber, int pageSize);
    Task<Result<Integrante>> EditarIntegrante(int idIntegrante, IntegranteRequest integrante);
    Task<Result> ExcluirIntegrante(int idIntegrante);
}