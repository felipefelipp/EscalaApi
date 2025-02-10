using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface ITipoIntegranteRepository
{
    Task<bool> InserirTipoIntegrante(List<TipoIntegranteDto> tipoIntegranteDto);
}