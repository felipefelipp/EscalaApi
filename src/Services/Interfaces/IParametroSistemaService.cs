using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IParametroSistemaService
{
    Task<Result<List<ParametroSistema>>> ListarAsync();
    Task<Result<string>> ObterRangeMaximoAsync();
    Task<Result<ParametroSistema>> AtualizarRangeMaximoAsync(RangeMaximoRequest request);
}
