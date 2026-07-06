using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IEstrategiaAlgoritmoService
{
    Task<Result<List<EstrategiaAlgoritmo>>> ListarAsync();
    Task<Result<EstrategiaAlgoritmo>> ObterPorIdAsync(int id);
}
