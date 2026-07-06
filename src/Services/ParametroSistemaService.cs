using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class ParametroSistemaService : IParametroSistemaService
{
    private readonly IParametroSistemaRepository _repository;

    public ParametroSistemaService(IParametroSistemaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ParametroSistema>>> ListarAsync()
    {
        var parametros = await _repository.ListarAsync();
        var resultado = parametros.Select(Map).ToList();
        GarantirRangeMaximoPadrao(resultado);
        return Result<List<ParametroSistema>>.Ok(resultado);
    }

    public async Task<Result<string>> ObterRangeMaximoAsync()
    {
        var parametro = await _repository.ObterPorChaveAsync(ValidadorRangeMaximo.ChaveParametro);
        var valor = ValidadorRangeMaximo.ResolverValorOuPadrao(parametro?.Valor);
        return Result<string>.Ok(valor);
    }

    public async Task<Result<ParametroSistema>> AtualizarRangeMaximoAsync(RangeMaximoRequest request)
    {
        if (!ValidadorRangeMaximo.EhValido(request.Valor))
        {
            var valores = string.Join(", ", ValidadorRangeMaximo.ValoresAceitos);
            return Result<ParametroSistema>.BadRequest([
                new Notification("Valor", $"Valor inválido. Valores aceitos: {valores}.")
            ]);
        }

        var valorNormalizado = ValidadorRangeMaximo.Normalizar(request.Valor);
        var existente = await _repository.ObterPorChaveAsync(ValidadorRangeMaximo.ChaveParametro);

        if (existente == null)
        {
            await _repository.InserirAsync(
                ValidadorRangeMaximo.ChaveParametro,
                valorNormalizado,
                "Range máximo permitido para configurações de escala.");
        }
        else
        {
            await _repository.AtualizarValorAsync(ValidadorRangeMaximo.ChaveParametro, valorNormalizado);
        }

        var atualizado = await _repository.ObterPorChaveAsync(ValidadorRangeMaximo.ChaveParametro);
        return Result<ParametroSistema>.Ok(Map(atualizado!));
    }

    private static void GarantirRangeMaximoPadrao(List<ParametroSistema> parametros)
    {
        var range = parametros.FirstOrDefault(p =>
            p.Chave.Equals(ValidadorRangeMaximo.ChaveParametro, StringComparison.OrdinalIgnoreCase));

        if (range == null)
        {
            parametros.Add(CriarRangeMaximoPadrao());
            return;
        }

        if (string.IsNullOrWhiteSpace(range.Valor))
            range.Valor = ValidadorRangeMaximo.ValorPadrao;
    }

    private static ParametroSistema CriarRangeMaximoPadrao() => new()
    {
        Id = 0,
        Chave = ValidadorRangeMaximo.ChaveParametro,
        Valor = ValidadorRangeMaximo.ValorPadrao,
        Descricao = "Range máximo permitido para configurações de escala (valor padrão em código).",
        DataAtualizacao = DateTime.UtcNow
    };

    private static ParametroSistema Map(Data.DTOs.ParametroSistemaDto dto) => new()
    {
        Id = dto.IdParametro,
        Chave = dto.Chave,
        Valor = dto.Valor,
        Descricao = dto.Descricao,
        DataAtualizacao = dto.DataAtualizacao
    };
}
