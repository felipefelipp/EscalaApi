using EscalaApi.Repositories;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services;
using EscalaApi.Services.Interfaces;
using EscalaApi.Data;

namespace EscalaApi.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseContext.Initialize(configuration);

        //Integrante
        services.AddScoped<IIntegranteService, IntegranteService>();
        services.AddScoped<IIntegranteRepository, IntegranteRepository>();

        
        //Escala
        services.AddScoped<IEscalaManagerService, EscalaManager>();
        services.AddScoped<IEscalaRepository, EscalaRepository>();

        
        //Tipo integrante 
        services.AddScoped<ITipoIntegranteRepository, TipoIntegranteRepository>();
        
        //Dias disponiveis
        services.AddScoped<IIntegranteDiasDisponiveisRepository, IntegranteDiasDisponiveisRepository>();
        
        return services;
    }

    public static void AddMappingServices(this IServiceCollection services)
    {
       // services.AddAutoMapper(typeof(MappingProfile));
    }
}