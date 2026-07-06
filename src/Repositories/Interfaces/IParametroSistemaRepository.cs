using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IParametroSistemaRepository
{
    Task<List<ParametroSistemaDto>> ListarAsync();
    Task<ParametroSistemaDto?> ObterPorChaveAsync(string chave);
    Task<bool> AtualizarValorAsync(string chave, string valor);
    Task<int> InserirAsync(string chave, string valor, string? descricao);
}
