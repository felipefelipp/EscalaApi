using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface ITipoIntegranteCatalogoService
{
    Task<Result<List<TipoIntegranteCatalogo>>> ListarAsync();
    Task<Result<TipoIntegranteCatalogo>> ObterPorIdAsync(int id);
    Task<Result<bool>> ExisteAsync();
    Task<Result<TipoIntegranteCatalogo>> InserirAsync(TipoIntegranteCatalogoRequest request);
    Task<Result<TipoIntegranteCatalogo>> AtualizarAsync(int id, TipoIntegranteCatalogoRequest request);
    Task<Result<bool>> ExcluirAsync(int id);
}
