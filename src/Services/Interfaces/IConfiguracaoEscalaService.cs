using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Services.Results;
using EscalaApi.Services.Rotacao;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Services.Interfaces;

public interface IConfiguracaoEscalaService
{
    Task<Result<List<ConfiguracaoEscala>>> ListarAsync();
    Task<Result<ConfiguracaoEscala>> ObterPorIdAsync(int id);
    Task<Result<List<DateTime>>> ObterDatasExpandidasAsync(int id);
    Task<Result<ConfiguracaoEscala>> InserirAsync(ConfiguracaoEscalaRequest request);
    Task<Result<ConfiguracaoEscala>> AtualizarAsync(int id, ConfiguracaoEscalaRequest request);
}

public interface IEscalaGeracaoService
{
    Task<Result<ResultadoPreview>> GerarPreviewAsync(GerarEscalaRequest request);
    Task<Result<PersistenciaPreviewResultado>> PersistirPreviewAsync(string token);
}
