using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface ITipoIntegranteRepository
{
    Task<bool> InserirTipoIntegrante(List<IntegranteDto> tipoIntegrante);
    Task<bool> AtualizarTipoIntegrante(List<IntegranteDto> tipoIntegrante);
    Task<bool> RemoverTipoIntegrante(int IdIntegrante);

}