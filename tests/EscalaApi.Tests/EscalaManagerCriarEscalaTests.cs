using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;
using Moq;

namespace EscalaApi.Tests;

public class EscalaManagerCriarEscalaTests
{
    private readonly Mock<IIntegranteRepository> _integranteRepoMock = new();
    private readonly Mock<IEscalaRepository> _escalaRepoMock = new();
    private readonly Mock<ITipoEscalaRepository> _tipoEscalaRepoMock = new();
    private readonly Mock<ITipoIntegranteCatalogoRepository> _tipoCatalogoRepoMock = new();

    public EscalaManagerCriarEscalaTests()
    {
        _tipoEscalaRepoMock.Setup(r => r.ObterTiposEscalaDisponiveis())
            .ReturnsAsync([1, 2]);

        _escalaRepoMock.Setup(r => r.ObterEscalas(It.IsAny<EscalaFiltro>()))
            .ReturnsAsync(new List<EscalaDto>());
    }

    [Fact]
    public async Task CriarEscala_ComImpedirMultiplosAtivo_NaoAlocaMesmoIntegranteEmMultiplasFuncoesNoMesmoDia()
    {
        var domingo = new DateTime(2026, 10, 4); // 04/10/2026 é Domingo

        var integrante1Tipo1 = new IntegranteDto
        {
            IdIntegrante = 1,
            Nome = "Lucas",
            TipoIntegrante = 1,
            DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday
        };

        var integrante1Tipo2 = new IntegranteDto
        {
            IdIntegrante = 1,
            Nome = "Lucas",
            TipoIntegrante = 2,
            DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday
        };

        var integrante2Tipo2 = new IntegranteDto
        {
            IdIntegrante = 2,
            Nome = "Mateus",
            TipoIntegrante = 2,
            DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday
        };

        _integranteRepoMock.Setup(r => r.ObterIntegrantes(It.Is<IntegranteFiltro>(f => f.TipoIntegrante == 1)))
            .ReturnsAsync(([integrante1Tipo1], 1));

        _integranteRepoMock.Setup(r => r.ObterIntegrantes(It.Is<IntegranteFiltro>(f => f.TipoIntegrante == 2)))
            .ReturnsAsync(([integrante1Tipo2, integrante2Tipo2], 2));

        var service = new EscalaManager(
            _integranteRepoMock.Object,
            _escalaRepoMock.Object,
            _tipoEscalaRepoMock.Object,
            _tipoCatalogoRepoMock.Object);

        var request = new EscalaIntegrantes(
            domingo,
            domingo,
            TipoEscala: [1, 2],
            DiasDaSemana: [DayOfWeek.Sunday],
            Persistir: false,
            ImpedirMultiplosTiposMesmoDia: true);

        var resultado = await service.CriarEscala(request);

        Assert.True(resultado.Sucess);
        Assert.Equal(2, resultado.Object!.Count);

        var alocadosNoDia = resultado.Object.Select(e => e.Integrante.IdIntegrante).ToList();
        Assert.Contains(1, alocadosNoDia);
        Assert.Contains(2, alocadosNoDia);
        Assert.Equal(2, alocadosNoDia.Distinct().Count());
    }

    [Fact]
    public async Task CriarEscala_ComImpedirMultiplosDesativado_PermiteAlocarMesmoIntegranteSeForUnicaOpcao()
    {
        var domingo = new DateTime(2026, 10, 4);

        var integrante1Tipo1 = new IntegranteDto
        {
            IdIntegrante = 1,
            Nome = "Lucas",
            TipoIntegrante = 1,
            DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday
        };

        var integrante1Tipo2 = new IntegranteDto
        {
            IdIntegrante = 1,
            Nome = "Lucas",
            TipoIntegrante = 2,
            DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday
        };

        _integranteRepoMock.Setup(r => r.ObterIntegrantes(It.Is<IntegranteFiltro>(f => f.TipoIntegrante == 1)))
            .ReturnsAsync(([integrante1Tipo1], 1));

        _integranteRepoMock.Setup(r => r.ObterIntegrantes(It.Is<IntegranteFiltro>(f => f.TipoIntegrante == 2)))
            .ReturnsAsync(([integrante1Tipo2], 1));

        var service = new EscalaManager(
            _integranteRepoMock.Object,
            _escalaRepoMock.Object,
            _tipoEscalaRepoMock.Object,
            _tipoCatalogoRepoMock.Object);

        var request = new EscalaIntegrantes(
            domingo,
            domingo,
            TipoEscala: [1, 2],
            DiasDaSemana: [DayOfWeek.Sunday],
            Persistir: false,
            ImpedirMultiplosTiposMesmoDia: false);

        var resultado = await service.CriarEscala(request);

        Assert.True(resultado.Sucess);
        Assert.Equal(2, resultado.Object!.Count);
        Assert.All(resultado.Object, e => Assert.Equal(1, e.Integrante.IdIntegrante));
    }
}
