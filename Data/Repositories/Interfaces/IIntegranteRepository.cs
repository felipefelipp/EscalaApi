using EscalaApi.Data.Entities;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Repositories.Interfaces;

public interface IIntegranteRepository
{
    Task<Integrante> ObterIntegrantePorId(int idIntegrante);
    Task<List<Integrante>?> ObterIntegrantesPorTipo(TipoIntegrante tipoIntegrante);
}