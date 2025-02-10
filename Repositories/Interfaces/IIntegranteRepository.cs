using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Repositories.Interfaces;

public interface IIntegranteRepository
{
    Task<Integrante> ObterIntegrantePorId(int idIntegrante);
    Task<List<Integrante>?> ObterIntegrantesPorTipo(TipoIntegrante tipoIntegrante);
    Task<int> InserirIntegrante(IntegranteDto integrante);
    Task<(IEnumerable<Integrante> integrantes, int total)> ObterIntegrantes(int pageNumber, int pageSize);
}