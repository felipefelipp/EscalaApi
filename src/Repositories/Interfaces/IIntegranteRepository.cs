using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Utils.Enums;

namespace EscalaApi.Repositories.Interfaces;

public interface IIntegranteRepository
{
    Task<Integrante> ObterIntegrantePorId(int idIntegrante);
    Task<List<Integrante>?> ObterIntegrantesPorTipo(int tipoIntegrante);
    Task<int> InserirIntegrante(IntegranteDto integrante);
    Task<IntegrantesResultDto> ObterIntegrantes(int pageNumber, int pageSize);
    Task<bool> AtualizarIntegrante(IntegranteDto integrante);
    Task<bool> RemoverIntegrante(int idIntegrante);
}