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

    [Fact]
    public async Task CriarEscala_ComEvitarConsecutivosAtivo_AlternaMinistroEmCultosConsecutivos()
    {
        // Cenário Quarta (07/10/2026) e Domingo (11/10/2026)
        var quarta = new DateTime(2026, 10, 7);
        var domingo = new DateTime(2026, 10, 11);

        // Sheylla (1) e Yasmin (2) disponíveis na quarta e no domingo para Tipo 1 (Ministro)
        var sheyllaQua = new IntegranteDto { IdIntegrante = 1, Nome = "Sheylla", TipoIntegrante = 1, DiaDaSemanaDisponivel = (int)DayOfWeek.Wednesday };
        var yasminQua = new IntegranteDto { IdIntegrante = 2, Nome = "Yasmin", TipoIntegrante = 1, DiaDaSemanaDisponivel = (int)DayOfWeek.Wednesday };
        var sheyllaDom = new IntegranteDto { IdIntegrante = 1, Nome = "Sheylla", TipoIntegrante = 1, DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday };
        var yasminDom = new IntegranteDto { IdIntegrante = 2, Nome = "Yasmin", TipoIntegrante = 1, DiaDaSemanaDisponivel = (int)DayOfWeek.Sunday };

        _integranteRepoMock.Setup(r => r.ObterIntegrantes(It.Is<IntegranteFiltro>(f => f.DiaDisponivel == DayOfWeek.Wednesday)))
            .ReturnsAsync(([sheyllaQua, yasminQua], 2));

        _integranteRepoMock.Setup(r => r.ObterIntegrantes(It.Is<IntegranteFiltro>(f => f.DiaDisponivel == DayOfWeek.Sunday)))
            .ReturnsAsync(([sheyllaDom, yasminDom], 2));

        var service = new EscalaManager(
            _integranteRepoMock.Object,
            _escalaRepoMock.Object,
            _tipoEscalaRepoMock.Object,
            _tipoCatalogoRepoMock.Object);

        var request = new EscalaIntegrantes(
            quarta,
            domingo,
            TipoEscala: [1],
            DiasDaSemana: [DayOfWeek.Wednesday, DayOfWeek.Sunday],
            Persistir: false,
            ImpedirMultiplosTiposMesmoDia: true,
            EvitarConsecutivosMesmaFuncao: true);

        var resultado = await service.CriarEscala(request);

        Assert.True(resultado.Sucess);
        Assert.Equal(2, resultado.Object!.Count);

        var ministroQuarta = resultado.Object.First(e => e.Data.Date == quarta).Integrante.IdIntegrante;
        var ministroDomingo = resultado.Object.First(e => e.Data.Date == domingo).Integrante.IdIntegrante;

        // Com EvitarConsecutivosMesmaFuncao = true, quem ministrou na quarta NÃO pode ministrar no domingo se o outro está disponível
        Assert.NotEqual(ministroQuarta, ministroDomingo);
    }
}
