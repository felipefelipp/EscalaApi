using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;
using EscalaApi.Services.Results;
using Flunt.Notifications;
using Moq;

namespace EscalaApi.Tests;

public class GatekeepingIntegranteTests
{
    [Fact]
    public async Task InserirIntegrante_SemTiposCadastrados_Retorna422()
    {
        var integranteRepo = new Mock<IIntegranteRepository>();
        var tipoJoinRepo = new Mock<ITipoIntegranteRepository>();
        var diasRepo = new Mock<IIntegranteDiasDisponiveisRepository>();
        var catalogoRepo = new Mock<ITipoIntegranteCatalogoRepository>();

        catalogoRepo.Setup(r => r.ContarAtivosAsync()).ReturnsAsync(0);

        var service = new IntegranteService(
            integranteRepo.Object,
            tipoJoinRepo.Object,
            diasRepo.Object,
            catalogoRepo.Object);

        var request = new Data.Request.IntegranteRequest
        {
            Nome = "Teste",
            TipoIntegrante = [1],
            DiasDaSemanaDisponiveis = [DayOfWeek.Monday]
        };

        var result = await service.InserirIntegrante(request);

        Assert.False(result.Sucess);
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, result.StatusCode);
    }
}
