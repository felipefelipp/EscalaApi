using EscalaApi.Data.Entities;
using EscalaApi.Data.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using EscalaApi.Utils.Enums;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class IntegranteService : IIntegranteService
{
    IIntegranteRepository _integranteRepository;

    public IntegranteService(IIntegranteRepository integranteRepository)
    {
        _integranteRepository = integranteRepository;
    }

    public async Task<Result<Integrante>> ObterIntegrantePorId(int idIntegrante)
    {
        var erros = new List<Notification>();

        var integrante = await _integranteRepository.ObterIntegrantePorId(idIntegrante);

        if (integrante == null)
        {
            erros.Add(new Notification(idIntegrante.ToString(), $"Integrante não encontrado."));
            return Result<Integrante>.NotFound(erros);
        }

        return Result<Integrante>.Ok(integrante);
    }

    public async Task<Result<List<Integrante>>> ObterIntegrantesPorTipo(TipoIntegrante tipoIntegrante)
    {
        var integrantes = await _integranteRepository.ObterIntegrantesPorTipo(tipoIntegrante);
        
        return Result<List<Integrante>>.Ok(integrantes);
    }
}