using EscalaApi.Repositories;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Rotacao;
using EscalaApi.Data;

namespace EscalaApi.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseContext.Initialize(configuration);

        services.AddScoped<IIntegranteService, IntegranteService>();
        services.AddScoped<IIntegranteRepository, IntegranteRepository>();

        services.AddScoped<IEscalaManagerService, EscalaManager>();
        services.AddScoped<IEscalaRepository, EscalaRepository>();
        services.AddScoped<IEscalaGeracaoService, EscalaGeracaoService>();

        services.AddScoped<ITipoIntegranteRepository, TipoIntegranteRepository>();
        services.AddScoped<IIntegranteDiasDisponiveisRepository, IntegranteDiasDisponiveisRepository>();

        services.AddScoped<ITipoIntegranteCatalogoRepository, TipoIntegranteCatalogoRepository>();
        services.AddScoped<ITipoIntegranteCatalogoService, TipoIntegranteCatalogoService>();

        services.AddScoped<ITipoEscalaRepository, TipoEscalaRepository>();

        services.AddScoped<IParametroSistemaService, ParametroSistemaService>();
        services.AddScoped<IParametroSistemaRepository, ParametroSistemaRepository>();

        services.AddScoped<IConfiguracaoEscalaRepository, ConfiguracaoEscalaRepository>();
        services.AddScoped<IConfiguracaoEscalaService, ConfiguracaoEscalaService>();

        services.AddSingleton<ResolvedorEstrategia>();
        services.AddSingleton<RelatorioBalanceamento>();
        services.AddScoped<SeletorDeIntegrante>();
        services.AddScoped<GeradorDePreview>();

        return services;
    }
}
