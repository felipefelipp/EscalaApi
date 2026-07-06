using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface ITipoGranularidadeService
{
    Task<Result<List<TipoGranularidade>>> ListarAsync();
    Task<Result<TipoGranularidade>> ObterPorIdAsync(int id);
}
