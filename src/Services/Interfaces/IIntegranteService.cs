using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IIntegranteService
{
    Task<Result<Integrante>> ObterIntegrantePorId(int idIntegrante);
    Task<Result<List<Integrante>>> ObterIntegrantesPorTipo(int tipoIntegrante);
    Task<Result<Integrante>> InserirIntegrante(IntegranteRequest integranteRequest);
    Task<Result<(List<Integrante> integrantes, int total)>> ObterIntegrantes(IntegranteFiltro filtro);
    Task<Result<Integrante>> AtualizarIntegrante(int idIntegrante, IntegranteRequest integrante);
    Task<Result<Integrante>> ExcluirIntegrante(int idIntegrante);
}