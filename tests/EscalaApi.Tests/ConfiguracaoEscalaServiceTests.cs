using System.Net;
using EscalaApi.Data.Entities;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;
using Moq;

namespace EscalaApi.Tests;

public class ConfiguracaoEscalaServiceTests
{
    private readonly Mock<IConfiguracaoEscalaRepository> _repositoryMock;
    private readonly Mock<IParametroSistemaRepository> _parametroRepositoryMock;
    private readonly ConfiguracaoEscalaService _service;

    public ConfiguracaoEscalaServiceTests()
    {
        _repositoryMock = new Mock<IConfiguracaoEscalaRepository>();
        _parametroRepositoryMock = new Mock<IParametroSistemaRepository>();
        _service = new ConfiguracaoEscalaService(_repositoryMock.Object, _parametroRepositoryMock.Object);
    }

    [Fact]
    public async Task ExcluirAsync_ConfiguracaoNaoEncontrada_DeveRetornarNotFound()
    {
        const int idInexistente = 999;
        _repositoryMock.Setup(r => r.ObterPorIdAsync(idInexistente))
            .ReturnsAsync((ConfiguracaoEscala?)null);

        var resultado = await _service.ExcluirAsync(idInexistente);

        Assert.False(resultado.Sucess);
        Assert.Equal(HttpStatusCode.NotFound, resultado.StatusCode);
        _repositoryMock.Verify(r => r.ContarEscalasVinculadasAsync(It.IsAny<int>()), Times.Never);
        _repositoryMock.Verify(r => r.ExcluirAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ExcluirAsync_ComEscalasGeradasVinculadas_DeveRetornarConflict()
    {
        const int idConfig = 10;
        var config = new ConfiguracaoEscala
        {
            IdConfiguracao = idConfig,
            Nome = "Escala Março",
            DataInicio = new DateTime(2026, 3, 1),
            DataFim = new DateTime(2026, 3, 31)
        };

        _repositoryMock.Setup(r => r.ObterPorIdAsync(idConfig))
            .ReturnsAsync(config);
        _repositoryMock.Setup(r => r.ContarEscalasVinculadasAsync(idConfig))
            .ReturnsAsync(3); // Possui 3 escalas geradas

        var resultado = await _service.ExcluirAsync(idConfig);

        Assert.False(resultado.Sucess);
        Assert.Equal(HttpStatusCode.Conflict, resultado.StatusCode);
        Assert.Contains(resultado.Notifications, n => n.Message.Contains("escalas geradas"));
        _repositoryMock.Verify(r => r.ExcluirAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ExcluirAsync_SemEscalasGeradas_DeveExcluirComSucesso()
    {
        const int idConfig = 20;
        var config = new ConfiguracaoEscala
        {
            IdConfiguracao = idConfig,
            Nome = "Escala Vazia Teste",
            DataInicio = new DateTime(2026, 4, 1),
            DataFim = new DateTime(2026, 4, 30)
        };

        _repositoryMock.Setup(r => r.ObterPorIdAsync(idConfig))
            .ReturnsAsync(config);
        _repositoryMock.Setup(r => r.ContarEscalasVinculadasAsync(idConfig))
            .ReturnsAsync(0); // Nenhuma escala gerada
        _repositoryMock.Setup(r => r.ExcluirAsync(idConfig))
            .ReturnsAsync(true);

        var resultado = await _service.ExcluirAsync(idConfig);

        Assert.True(resultado.Sucess);
        Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        Assert.True(resultado.Object);
        _repositoryMock.Verify(r => r.ExcluirAsync(idConfig), Times.Once);
    }
}
