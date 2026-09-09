using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using EscalaApi.Services.Rotacao;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class ConfiguracaoEscalaService : IConfiguracaoEscalaService
{
    private readonly IConfiguracaoEscalaRepository _repository;
    private readonly IParametroSistemaRepository _parametroRepository;

    public ConfiguracaoEscalaService(
        IConfiguracaoEscalaRepository repository,
        IParametroSistemaRepository parametroRepository)
    {
        _repository = repository;
        _parametroRepository = parametroRepository;
    }

    public async Task<Result<List<ConfiguracaoEscala>>> ListarAsync()
    {
        var configs = await _repository.ListarAsync();
        return Result<List<ConfiguracaoEscala>>.Ok(configs);
    }

    public async Task<Result<ConfiguracaoEscala>> ObterPorIdAsync(int id)
    {
        var config = await _repository.ObterPorIdAsync(id);
        if (config is null)
            return Result<ConfiguracaoEscala>.NotFound([new Notification("Id", "Configuração não encontrada.")]);
        return Result<ConfiguracaoEscala>.Ok(config);
    }

    public async Task<Result<List<DateTime>>> ObterDatasExpandidasAsync(int id)
    {
        var configResult = await ObterPorIdAsync(id);
        if (!configResult.Sucess) return Result<List<DateTime>>.NotFound(configResult.Notifications.ToList());

        var config = configResult.Object!;
        var dias = config.ValoresRecorrentes.Select(v => (DayOfWeek)v).ToList();
        var datas = ExpansorDeDatas.Expand(config.DataInicio, config.DataFim, dias);
        return Result<List<DateTime>>.Ok(datas);
    }

    public async Task<Result<ConfiguracaoEscala>> InserirAsync(ConfiguracaoEscalaRequest request)
    {
        var erros = await ValidarAsync(request);
        if (erros.Count > 0) return Result<ConfiguracaoEscala>.UnprocessableEntity(erros);

        var id = await _repository.InserirAsync(request);
        await _repository.InserirSlotsAsync(id, request.ValoresRecorrentes);
        await _repository.InserirTiposAsync(id, request.TiposIntegrante);

        var criada = await _repository.ObterPorIdAsync(id);
        return Result<ConfiguracaoEscala>.Created(criada!);
    }

    public async Task<Result<ConfiguracaoEscala>> AtualizarAsync(int id, ConfiguracaoEscalaRequest request)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente is null)
            return Result<ConfiguracaoEscala>.NotFound([new Notification("Id", "Configuração não encontrada.")]);

        var erros = await ValidarAsync(request);
        if (erros.Count > 0) return Result<ConfiguracaoEscala>.UnprocessableEntity(erros);

        await _repository.AtualizarAsync(id, request);
        await _repository.RemoverSlotsETiposAsync(id);
        await _repository.InserirSlotsAsync(id, request.ValoresRecorrentes);
        await _repository.InserirTiposAsync(id, request.TiposIntegrante);

        var atualizada = await _repository.ObterPorIdAsync(id);
        return Result<ConfiguracaoEscala>.Ok(atualizada!);
    }

    private async Task<List<Notification>> ValidarAsync(ConfiguracaoEscalaRequest request)
    {
        var erros = new List<Notification>();

        if (string.IsNullOrWhiteSpace(request.Nome))
            erros.Add(new Notification("Nome", "Nome é obrigatório."));
        if (request.DataInicio > request.DataFim)
            erros.Add(new Notification("Data", "Data de início não pode ser maior que data fim."));
        if (request.ValoresRecorrentes.Count == 0)
            erros.Add(new Notification("ValoresRecorrentes", "Informe ao menos um dia recorrente."));
        if (request.TiposIntegrante.Count == 0)
            erros.Add(new Notification("TiposIntegrante", "Informe ao menos um tipo de integrante."));

        var parametro = await _parametroRepository.ObterPorChaveAsync("range_maximo_escala");
        var rangeCodigo = string.IsNullOrWhiteSpace(parametro?.Valor) ? "mensal" : parametro!.Valor;
        if (!ValidadorRangeMaximo.RangeValido(request.DataInicio, request.DataFim, rangeCodigo, out var diasPermitidos))
            erros.Add(new Notification("RangeExcedido",
                $"O intervalo excede o máximo configurado ({rangeCodigo}, {diasPermitidos} dias)."));

        return erros;
    }
}
