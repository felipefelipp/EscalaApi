using System.Net;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;
using Moq;

namespace EscalaApi.Tests;

public class EscalaManagerExcluirEditarTests
{
    private readonly Mock<IIntegranteRepository> _integranteRepoMock = new();
    private readonly Mock<IEscalaRepository> _escalaRepoMock = new();
    private readonly Mock<ITipoEscalaRepository> _tipoEscalaRepoMock = new();
    private readonly Mock<ITipoIntegranteCatalogoRepository> _tipoCatalogoRepoMock = new();

    private EscalaManager CriarService()
    {
        return new EscalaManager(
            _integranteRepoMock.Object,
            _escalaRepoMock.Object,
            _tipoEscalaRepoMock.Object,
            _tipoCatalogoRepoMock.Object);
    }

    [Fact]
    public async Task ExcluirEscala_ComIdValidoEExistente_DeveRetornarSucesso()
    {
        // Arrange
        int idEscala = 10;
        _escalaRepoMock.Setup(r => r.ObterEscalaPorId(idEscala))
            .ReturnsAsync(new EscalaDto { IdEscala = idEscala, IdIntegrante = 1 });
        _escalaRepoMock.Setup(r => r.ExcluirEscala(idEscala))
            .ReturnsAsync(true);

        var service = CriarService();

        // Act
        var result = await service.ExcluirEscala(idEscala);

        // Assert
        Assert.True(result.Sucess);
        Assert.True(result.Object);
        _escalaRepoMock.Verify(r => r.ExcluirEscala(idEscala), Times.Once);
    }

    [Fact]
    public async Task ExcluirEscala_ComIdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        int idEscala = 999;
        _escalaRepoMock.Setup(r => r.ObterEscalaPorId(idEscala))
            .ReturnsAsync((EscalaDto?)null);

        var service = CriarService();

        // Act
        var result = await service.ExcluirEscala(idEscala);

        // Assert
        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        _escalaRepoMock.Verify(r => r.ExcluirEscala(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ExcluirEscala_ComIdInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var service = CriarService();

        // Act
        var result = await service.ExcluirEscala(0);

        // Assert
        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task ExcluirEscalasEmLote_ComListaValida_DeveRetornarTotalExcluidas()
    {
        // Arrange
        var ids = new List<int> { 1, 2, 3 };
        _escalaRepoMock.Setup(r => r.ExcluirEscalasEmLote(It.Is<List<int>>(l => l.Count == 3)))
            .ReturnsAsync(3);

        var service = CriarService();

        // Act
        var result = await service.ExcluirEscalasEmLote(ids);

        // Assert
        Assert.True(result.Sucess);
        Assert.Equal(3, result.Object);
        _escalaRepoMock.Verify(r => r.ExcluirEscalasEmLote(It.IsAny<List<int>>()), Times.Once);
    }

    [Fact]
    public async Task ExcluirEscalasEmLote_ComListaVazia_DeveRetornarBadRequest()
    {
        // Arrange
        var service = CriarService();

        // Act
        var result = await service.ExcluirEscalasEmLote(new List<int>());

        // Assert
        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task EditarEscala_ComDadosValidos_DeveAtualizarComSucesso()
    {
        // Arrange
        int idEscala = 5;
        var request = new EscalaIntegrante(1, new DateTime(2026, 10, 1), 2);

        _escalaRepoMock.Setup(r => r.ObterEscalaPorId(idEscala))
            .ReturnsAsync(new EscalaDto { IdEscala = idEscala, IdIntegrante = 1 });
        _integranteRepoMock.Setup(r => r.ObterIntegrantePorId(1))
            .ReturnsAsync([new IntegranteDto { IdIntegrante = 1, Nome = "Felipe" }]);
        _tipoEscalaRepoMock.Setup(r => r.ObterTiposEscalaDisponiveis())
            .ReturnsAsync([1, 2, 3]);
        _escalaRepoMock.Setup(r => r.AtualizarEscala(idEscala, It.IsAny<EscalaDto>()))
            .ReturnsAsync(true);

        var service = CriarService();

        // Act
        var result = await service.EditarEscala(idEscala, request);

        // Assert
        Assert.True(result.Sucess);
        Assert.Equal(request.idIntegrante, result.Object.idIntegrante);
        _escalaRepoMock.Verify(r => r.AtualizarEscala(idEscala, It.IsAny<EscalaDto>()), Times.Once);
    }

    [Fact]
    public async Task EditarEscala_ComEscalaInexistente_DeveRetornarNotFound()
    {
        // Arrange
        int idEscala = 999;
        var request = new EscalaIntegrante(1, DateTime.Today, 2);

        _escalaRepoMock.Setup(r => r.ObterEscalaPorId(idEscala))
            .ReturnsAsync((EscalaDto?)null);

        var service = CriarService();

        // Act
        var result = await service.EditarEscala(idEscala, request);

        // Assert
        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task EditarEscala_ComIntegranteInexistente_DeveRetornarBadRequest()
    {
        // Arrange
        int idEscala = 5;
        var request = new EscalaIntegrante(999, DateTime.Today, 2);

        _escalaRepoMock.Setup(r => r.ObterEscalaPorId(idEscala))
            .ReturnsAsync(new EscalaDto { IdEscala = idEscala });
        _integranteRepoMock.Setup(r => r.ObterIntegrantePorId(999))
            .ReturnsAsync(new List<IntegranteDto>());

        var service = CriarService();

        // Act
        var result = await service.EditarEscala(idEscala, request);

        // Assert
        Assert.False(result.Sucess);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}
