using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
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

        var integranteDto = await _integranteRepository.ObterIntegrantePorId(idIntegrante);

        if (integranteDto == null || integranteDto.Count == 0)
        {
            erros.Add(new Notification(idIntegrante.ToString(), $"Integrante não encontrado."));
            return Result<Integrante>.NotFound(erros);
        }

        var integrante = integranteDto.ParaIntegrante();

        return Result<Integrante>.Ok(integrante);
    }

    public async Task<Result<List<Integrante>>> ObterIntegrantesPorTipo(int tipoIntegrante)
    {
        var erros = new List<Notification>();
        if (tipoIntegrante < 0)

        {
            erros.Add(new Notification(tipoIntegrante.ToString(), "Tipo de integrante inválido."));
            return Result<List<Integrante>>.BadRequest(erros);
        }

        var integrantesDto = await _integranteRepository.ObterIntegrantesPorTipo(tipoIntegrante);

        if (integrantesDto == null || integrantesDto.Count == 0)
        {
            erros.Add(new Notification(tipoIntegrante.ToString(), "Nenhum integrante encontrado para o tipo especificado."));
            return Result<List<Integrante>>.NotFound(erros);
        }

        var integrantes = integrantesDto.ParaIntegrantes();

        return Result<List<Integrante>>.Ok(integrantes);
    }

    public async Task<Result<(List<Integrante>, int)>> ObterIntegrantes(IntegranteFiltro filtro)
    {
        var erros = new List<Notification>();

        if (filtro.Skip < 0 || filtro.Take <= 0)
        {
            erros.Add(new Notification("Skip/Take", "Valores inválidos para paginação."));
            return Result<(List<Integrante>, int)>.BadRequest(erros);
        }

        if (filtro.Take > 100)
        {
            erros.Add(new Notification("Take", "O valor máximo permitido para 'take' é 100."));
            return Result<(List<Integrante>, int)>.BadRequest(erros);
        }

        var integrantesDto = await _integranteRepository.ObterIntegrantes(filtro);

        var integrantes = integrantesDto.integrantes.ParaIntegrantes();
        var total = integrantesDto.total;

        return Result<(List<Integrante>, int)>.Ok((integrantes, total));
    }

    public async Task<Result<Integrante>> AtualizarIntegrante(int idIntegrante, IntegranteRequest integrante)
    {
        var erros = new List<Notification>();
        if (idIntegrante < 0)
            erros.Add(new Notification(idIntegrante.ToString(), "Id inexistente."));

        var integranteEncontradoDto = await _integranteRepository.ObterIntegrantePorId(idIntegrante);

        if (integranteEncontradoDto == null || integranteEncontradoDto.Count == 0)
        {
            erros.Add(new Notification("", $"Integrante não encontrado."));
            return Result<Integrante>.NotFound(erros);
        }

        if (erros.Count != 0)
            return Result<Integrante>.BadRequest(erros);


        var integranteEncontrado = integranteEncontradoDto.ParaIntegrante();

        integranteEncontrado.Nome = integrante.Nome;
        integranteEncontrado.DiasDaSemanaDisponiveis = integrante.DiasDaSemanaDisponiveis;
        integranteEncontrado.TipoIntegrante = integrante.TipoIntegrante;

        var integranteDto = integranteEncontrado.ParaDtos();

        if (integranteDto == null || integranteDto.Count == 0)
        {
            erros.Add(new Notification("", "Dados do integrante inválidos para atualização."));
            return Result<Integrante>.BadRequest(erros);
        }

        var integranteAtualizado = await _integranteRepository.AtualizarIntegrante(integranteDto.FirstOrDefault());

        if (!integranteAtualizado)
        {
            erros.Add(new Notification("", $"Não foi possível atualizar o Integrante."));
            return Result<Integrante>.NotFound(erros);
        }

        var tipoIntegranteDto = integranteDto.DistinctBy(x => x.IdIntegrante)
            .Select(x => new IntegranteDto
            {
                IdIntegrante = x.IdIntegrante,
                TipoIntegrante = x.TipoIntegrante
            }).ToList();

        var tipoIntegranteAtualizado = await _tipoIntegranteRepository.AtualizarTipoIntegrante(tipoIntegranteDto);

        if (!tipoIntegranteAtualizado)
        {
            erros.Add(new Notification("", $"Não foi possível atualizar o tipo do Integrante."));
            return Result<Integrante>.NotFound(erros);
        }

        var diasDisponiveisIntegranteAtualizado =
            await _diasDisponiveisRepositoryRepository.AtualizarDiasDisponiveis(integranteDto);

        if (!diasDisponiveisIntegranteAtualizado)
        {
            erros.Add(new Notification("", $"Não foi possível atualizar os dias disponíveis do Integrante."));
            return Result<Integrante>.NotFound(erros);
        }

        return Result<Integrante>.Ok(integranteEncontrado);
    }

    public async Task<Result<Integrante>> InserirIntegrante(IntegranteRequest integranteRequest)
    {
        var erros = new List<Notification>();
        var integranteDto = integranteRequest.ParaDtos();

        if (integranteDto == null || integranteDto.Count == 0)
        {
            erros.Add(new Notification("", "Dados do integrante inválidos para inserção."));
            return Result<Integrante>.BadRequest(erros);
        }

        var idIntegranteInserido = await _integranteRepository.InserirIntegrante(integranteDto.First());

        if (idIntegranteInserido == 0)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(), $"Não foi possível inserir integrante."));
            return Result<Integrante>.BadRequest(erros);
        }

        integranteDto.First().IdIntegrante = idIntegranteInserido;

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

        var integrante = integranteRequest.ParaIntegrante(idIntegranteInserido);

        return Result<Integrante>.Ok(integrante);
    }

    public async Task<Result<Integrante>> ExcluirIntegrante(int idIntegrante)
    {
        var erros = new List<Notification>();
        if (idIntegrante < 0)
            erros.Add(new Notification(idIntegrante.ToString(), "Id inexistente."));

        var integranteEncontrado = await _integranteRepository.ObterIntegrantePorId(idIntegrante);

        if (integranteEncontrado is null || integranteEncontrado.Count == 0)
        {
            erros.Add(new Notification("", $"Integrante não existe."));
            return Result<Integrante>.NotFound(erros);
        }

        if (erros.Count != 0)
            return Result<Integrante>.BadRequest(erros);

        var diasDisponiveisIntegranteRemovido =
            await _diasDisponiveisRepositoryRepository.RemoverDiasDisponiveis(idIntegrante);

        if (!diasDisponiveisIntegranteRemovido)
        {
            erros.Add(new Notification(idIntegrante.ToString(),
                $"Não foi possível remover os dias disponiveis do integrante."));
            return Result<Integrante>.BadRequest(erros);
        }

        var tipoIntegranteRemovido = await _tipoIntegranteRepository.RemoverTipoIntegrante(idIntegrante);

        if (!tipoIntegranteRemovido)
        {
            erros.Add(new Notification(idIntegrante.ToString(), $"Não foi possível remover tipo integrante."));
            return Result<Integrante>.BadRequest(erros);
        }

        var idIntegranteInserido = await _integranteRepository.RemoverIntegrante(idIntegrante);

        if (!idIntegranteInserido)
        {
            erros.Add(new Notification(idIntegranteInserido.ToString(), $"Não foi possível remover integrante."));
            return Result<Integrante>.BadRequest(erros);
        }

        return Result<Integrante>.NoContent();
    }
}