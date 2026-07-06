using EscalaApi.Data.DTOs;
using EscalaApi.Data.Request;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;

namespace EscalaApi.Tests;

public class ParametroSistemaServiceTests
{
    [Fact]
    public async Task ObterRangeMaximoAsync_SemParametroNoBanco_DeveRetornarMensal()
    {
        var repository = new FakeParametroSistemaRepository();
        var service = new ParametroSistemaService(repository);

        var resultado = await service.ObterRangeMaximoAsync();

        Assert.True(resultado.Sucess);
        Assert.Equal("mensal", resultado.Object);
    }

    [Fact]
    public async Task ListarAsync_SemRangeMaximoNoBanco_DeveIncluirPadraoMensal()
    {
        var repository = new FakeParametroSistemaRepository();
        var service = new ParametroSistemaService(repository);

        var resultado = await service.ListarAsync();

        Assert.True(resultado.Sucess);
        var range = resultado.Object!.First(p => p.Chave == ValidadorRangeMaximo.ChaveParametro);
        Assert.Equal("mensal", range.Valor);
    }

    [Fact]
    public async Task AtualizarRangeMaximoAsync_ValorInvalido_DeveRetornarBadRequest()
    {
        var repository = new FakeParametroSistemaRepository();
        var service = new ParametroSistemaService(repository);

        var resultado = await service.AtualizarRangeMaximoAsync(new RangeMaximoRequest { Valor = "invalido" });

        Assert.False(resultado.Sucess);
    }

    [Fact]
    public async Task AtualizarRangeMaximoAsync_ValorValido_DevePersistirNormalizado()
    {
        var repository = new FakeParametroSistemaRepository();
        var service = new ParametroSistemaService(repository);

        var resultado = await service.AtualizarRangeMaximoAsync(new RangeMaximoRequest { Valor = "Trimestral" });

        Assert.True(resultado.Sucess);
        Assert.Equal("trimestral", resultado.Object!.Valor);
    }

    private sealed class FakeParametroSistemaRepository : IParametroSistemaRepository
    {
        private readonly Dictionary<string, ParametroSistemaDto> _parametros = new(StringComparer.OrdinalIgnoreCase);

        public Task<List<ParametroSistemaDto>> ListarAsync() =>
            Task.FromResult(_parametros.Values.ToList());

        public Task<ParametroSistemaDto?> ObterPorChaveAsync(string chave) =>
            Task.FromResult(_parametros.TryGetValue(chave, out var parametro) ? parametro : null);

        public Task<bool> AtualizarValorAsync(string chave, string valor)
        {
            if (!_parametros.TryGetValue(chave, out var parametro))
                return Task.FromResult(false);

            parametro.Valor = valor;
            parametro.DataAtualizacao = DateTime.UtcNow;
            return Task.FromResult(true);
        }

        public Task<int> InserirAsync(string chave, string valor, string? descricao)
        {
            var id = _parametros.Count + 1;
            _parametros[chave] = new ParametroSistemaDto
            {
                IdParametro = id,
                Chave = chave,
                Valor = valor,
                Descricao = descricao,
                DataAtualizacao = DateTime.UtcNow
            };
            return Task.FromResult(id);
        }
    }
}
