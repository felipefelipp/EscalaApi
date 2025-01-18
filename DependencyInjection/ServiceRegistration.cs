using EscalaApi.Data.Repositories;
using EscalaApi.Data.Repositories.Interfaces;
using EscalaApi.Mappers;
using EscalaApi.Services;
using EscalaApi.Services.Interfaces;

namespace EscalaApi.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        //Integrante
        services.AddScoped<IIntegranteService, IntegranteService>();
        services.AddScoped<IIntegranteRepository, IntegranteRepository>();
        
        //Escala
        services.AddScoped<IEscalaManagerService, EscalaManager>();
        services.AddScoped<IEscalaRepository, EscalaRepository>();

        
        return services;
    }

    public static void AddMappingServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
    }
}