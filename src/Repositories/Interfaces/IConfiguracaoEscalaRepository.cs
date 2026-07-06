using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;

namespace EscalaApi.Repositories.Interfaces;

public interface IConfiguracaoEscalaRepository
{
    Task<int> InserirAsync(ConfiguracaoEscalaRequest request);
    Task<ConfiguracaoEscala?> ObterPorIdAsync(int id);
    Task<List<ConfiguracaoEscala>> ListarAsync();
    Task AtualizarAsync(int id, ConfiguracaoEscalaRequest request);
    Task InserirSlotsAsync(int idConfiguracao, IEnumerable<int> slots);
    Task InserirTiposAsync(int idConfiguracao, IEnumerable<int> tipos);
    Task<List<int>> ObterSlotsAsync(int id);
    Task<List<int>> ObterTiposAsync(int id);
    Task MarcarEstrategiaImutavelAsync(int id);
    Task RemoverSlotsETiposAsync(int id);
}
