using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using EscalaApi.Utils.Enums;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class IntegranteService : IIntegranteService
{
    IIntegranteRepository _integranteRepository;
    ITipoIntegranteRepository _tipoIntegranteRepository;
    IIntegranteDiasDisponiveisRepository _diasDisponiveisRepositoryRepository;

    public IntegranteService(IIntegranteRepository integranteRepository,
        ITipoIntegranteRepository tipoIntegranteRepository,
        IIntegranteDiasDisponiveisRepository diasDisponiveisRepositoryRepository)
    {
        _integranteRepository = integranteRepository;
        _tipoIntegranteRepository = tipoIntegranteRepository;
        _diasDisponiveisRepositoryRepository = diasDisponiveisRepositoryRepository;
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
    
    public async Task<Result<(IEnumerable<Integrante> integrantes, int total)>> ObterIntegrantes(int pageNumber, int pageSize)
    {
        var integrantes = await _integranteRepository.ObterIntegrantes(pageNumber, pageSize);

        return Result<(IEnumerable<Integrante> integrantes, int total)>.Ok(integrantes);
    }

    public async Task<Result<Integrante>> RegistrarIntegrante(IntegranteRequest integranteRequest)
    {
        var erros = new List<Notification>();
        var integranteDto = new IntegranteDto()
        {
            Nome = integranteRequest.Nome,
        };

        var idIntegranteInserido = await _integranteRepository.InserirIntegrante(integranteDto);

        if (idIntegranteInserido == 0)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(), $"Não foi possível inserir integrante."));
            Result<Integrante>.BadRequest(erros);
        }

        var tipoIntegranteDto = new List<TipoIntegranteDto>();
        foreach (var tipo in integranteRequest.TipoIntegrante)
        {
            tipoIntegranteDto.Add(new TipoIntegranteDto()
            {
                IdIntegrante = idIntegranteInserido,
                TipoIntegrante = (int)tipo
            });
        }

        var tipoIntegranteInserido = await _tipoIntegranteRepository.InserirTipoIntegrante(tipoIntegranteDto);

        if (!tipoIntegranteInserido)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(), $"Não foi possível inserir tipo integrante."));
            Result<Integrante>.BadRequest(erros);
        }

        var diasDisponiveisIntegranteDto = new List<IntegranteDiasDisponiveisDto>();
        foreach (var diaDisponivel in integranteRequest.DiasDisponiveis)
        {
            diasDisponiveisIntegranteDto.Add(new IntegranteDiasDisponiveisDto()
            {
                IdIntegrante = idIntegranteInserido,
                DiaDisponivel = (int)diaDisponivel
            });
        }

        var diasDisponiveisIntegranteInserido =
            await _diasDisponiveisRepositoryRepository.InserirDiasDisponiveis(diasDisponiveisIntegranteDto);

        if (!diasDisponiveisIntegranteInserido)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(),
                $"Não foi possível inserir os dias disponiveis."));
            Result<Integrante>.BadRequest(erros);
        }

        var integrante = new Integrante(idIntegranteInserido,
            integranteRequest.Nome,
            integranteRequest.DiasDisponiveis,
            integranteRequest.TipoIntegrante);

        return Result<Integrante>.Ok(integrante);
    }
}