using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface ITipoIntegranteRepository
{
    Task<bool> InserirTipoIntegrante(IntegranteDto tipoIntegrante);
    Task<bool> AtualizarTipoIntegrante(IntegranteDto tipoIntegrante);
    Task<bool> RemoverTipoIntegrante(int IdIntegrante);

}