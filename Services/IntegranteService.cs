using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Mappers;
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

    public async Task<Result<IntegrantesResultDto>> ObterIntegrantes(int pageNumber, int pageSize)
    {
        var integrantes = await _integranteRepository.ObterIntegrantes(pageNumber, pageSize);

        return Result<IntegrantesResultDto>.Ok(integrantes);
    }

    public async Task<Result> EditarIntegrante(int idIntegrante, IntegranteRequest integrante)
    {
        var erros = new List<Notification>();
        if (idIntegrante < 0)
            erros.Add(new Notification(idIntegrante.ToString(), "Id inexistente."));

        var integranteEncontrado = await _integranteRepository.ObterIntegrantePorId(idIntegrante);

        if (integranteEncontrado == null)
        {
            erros.Add(new Notification("", $"Integrante não encontrado."));
            return Result.NotFound(erros);
        }

        integranteEncontrado.Nome = integrante.Nome;
        integranteEncontrado.DiasDaSemanaDisponiveis = integrante.DiasDaSemanaDisponiveis;
        integranteEncontrado.TipoIntegrante = integrante.TipoIntegrante;
        var integranteDto = integranteEncontrado.ParaDto();

        if (erros.Any())
            return Result.BadRequest(erros);

        var integranteAtualizado = await _integranteRepository.AtualizarIntegrante(integranteDto);

        if (!integranteAtualizado)
        {
            erros.Add(new Notification("", $"Não foi possível atualizar o Integrante."));
            return Result.NotFound(erros);
        }

        var tipoIntegranteAtualizado = await _tipoIntegranteRepository.AtualizarTipoIntegrante(integranteDto);
        if (!tipoIntegranteAtualizado)
        {
            erros.Add(new Notification("", $"Não foi possível atualizar o tipo do Integrante."));
            return Result.NotFound(erros);
        }

        var diasDisponiveisIntegranteAtualizado =
            await _diasDisponiveisRepositoryRepository.AtualizarDiasDisponiveis(integranteDto);
        if (!diasDisponiveisIntegranteAtualizado)
        {
            erros.Add(new Notification("", $"Não foi possível atualizar os dias disponíveis do Integrante."));
            return Result.NotFound(erros);
        }

        return Result.NoContent();
    }

    public async Task<Result<Integrante>> RegistrarIntegrante(IntegranteRequest integranteRequest)
    {
        var erros = new List<Notification>();
        var integranteDto = integranteRequest.ParaDto();

        var idIntegranteInserido = await _integranteRepository.InserirIntegrante(integranteDto);

        if (idIntegranteInserido == 0)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(), $"Não foi possível inserir integrante."));
            Result<Integrante>.BadRequest(erros);
        }
        
        integranteDto.IdIntegrante = idIntegranteInserido;
        
        var tipoIntegranteInserido = await _tipoIntegranteRepository.InserirTipoIntegrante(integranteDto);

        if (!tipoIntegranteInserido)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(), $"Não foi possível inserir tipo integrante."));
            return Result<Integrante>.BadRequest(erros);
        }

        var diasDisponiveisIntegranteInserido =
            await _diasDisponiveisRepositoryRepository.InserirDiasDisponiveis(integranteDto);

        if (!diasDisponiveisIntegranteInserido)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(),
                $"Não foi possível inserir os dias disponiveis."));
            return Result<Integrante>.BadRequest(erros);
        }

        var integrante = integranteRequest.ParaIntegrante();
        integrante.IdIntegrante = idIntegranteInserido;

        return Result<Integrante>.Ok(integrante);
    }
}