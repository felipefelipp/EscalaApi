using EscalaApi.Data.DTOs;
using EscalaApi.Data.Request;

namespace EscalaApi.Repositories.Interfaces;

public interface ITipoIntegranteCatalogoRepository
{
    Task<List<TipoIntegranteCatalogoDto>> ListarAsync(bool apenasAtivos = true);
    Task<TipoIntegranteCatalogoDto?> ObterPorIdAsync(int id);
    Task<int> ContarAtivosAsync();
    Task<int> InserirAsync(TipoIntegranteCatalogoRequest request);
    Task<bool> AtualizarAsync(int id, TipoIntegranteCatalogoRequest request);
    Task<bool> ExcluirSoftAsync(int id);
    Task<bool> ExisteNomeAsync(string nome, int idExcluir = 0);
}
