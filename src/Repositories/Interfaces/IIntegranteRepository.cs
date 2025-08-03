using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;

namespace EscalaApi.Repositories.Interfaces;

public interface IIntegranteRepository
{
    Task<List<IntegranteDto>?> ObterIntegrantePorId(int idIntegrante);
    Task<List<IntegranteDto>?> ObterIntegrantesPorTipo(int tipoIntegrante);
    Task<int> InserirIntegrante(IntegranteDto integrante);
    Task<(List<IntegranteDto> integrantes, int total)> ObterIntegrantes(IntegranteFiltro filtro);
    Task<bool> AtualizarIntegrante(IntegranteDto integrante);
    Task<bool> RemoverIntegrante(int idIntegrante);
}