using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using EscalaApi.Utils.Enums;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class EscalaManager : IEscalaManagerService
{
    public IIntegranteRepository _integranteRepository;
    public IEscalaRepository _escalaRepository;

    public EscalaManager(IIntegranteRepository integranteRepository, IEscalaRepository escalaRepository)
    {
        _integranteRepository = integranteRepository;
        _escalaRepository = escalaRepository;
    }

    public async Task<Result<Escala>> ObterEscalaPorId(int idEscala)
    {
        var erros = new List<Notification>();
        var escaladto = await _escalaRepository.ObterEscalaPorId(idEscala);

        if (escaladto == null)
        {
            erros.Add(new Notification(null, $"Escala não encontrada."));
            return Result<Escala>.NotFound(erros);
        }

        var escala = escaladto.ParaEscala();

        return Result<Escala>.Ok(escala);
    }

    public async Task<Result<List<Escala>>> CriarEscala(EscalaIntegrantes escala)
    {
        var escalaIntegrantes = new List<Escala>();
        var erros = new List<Notification>();

        foreach (var dia in escala.DiasDaSemana)
        {
            var DiasDaEscala = ObterDiasEscala(escala.DataInicio.Date, escala.DataFim.Date, dia);

            if (escala.TipoEscala is not null)
            {
                await ProcessarEscalas(escala.TipoEscala, DiasDaEscala, escalaIntegrantes, erros);
            }
        }

        if (erros.Any())
            return Result<List<Escala>>.BadRequest(erros);

        var escalaDto = escalaIntegrantes.ParaListaEscalaDto();

        await _escalaRepository.InserirEscala(escalaDto);

        return Result<List<Escala>>.Ok(escalaIntegrantes);
    }

    public async Task<Result<List<Escala>>> ObterEscalas()
    {
        var escalasDto = await _escalaRepository.ObterEscalas();
        var escalas = escalasDto.ParaListaEscala();

        return Result<List<Escala>>.Ok(escalas);
    }

    public async Task<Result<EscalaIntegrante>> EditarEscala(int id, EscalaIntegrante escala)
    {
        var erros = new List<Notification>();
        var escaladto = await _escalaRepository.ObterEscalaPorId(id);
        if (escaladto == null)
        {
            erros.Add(new Notification(null, $"Escala não encontrada."));
            return Result<EscalaIntegrante>.NotFound(erros);
        }

        escaladto.TipoEscala = (int)escala.TipoEscala;
        escaladto.IdIntegrante = escala.idIntegrante;
        escaladto.Data = escala.Data;
        var escalaAtualizada = await _escalaRepository.AtualizarEscala(id, escaladto);
        if (!escalaAtualizada)
        {
            erros.Add(new Notification(id.ToString(), $"Não foi possível atualizar a escala."));
            return Result<EscalaIntegrante>.NotFound(erros);
        }

        return Result<EscalaIntegrante>.Ok(escala);
    }

    public static List<DiaSemana> ObterDiasEscala(DateTime dataInicio, DateTime dataFim, DayOfWeek diaDaSemana)
    {
        var diasEscala = new List<DiaSemana>();

        while (dataInicio.DayOfWeek != diaDaSemana)
        {
            dataInicio = dataInicio.AddDays(1);
        }

        while (dataInicio <= dataFim)
        {
            if (dataInicio.DayOfWeek == diaDaSemana)
            {
                diasEscala.Add(new DiaSemana(dataInicio, diaDaSemana));
            }

            dataInicio = dataInicio.AddDays(1);
        }

        return diasEscala;
    }

    private async Task ProcessarEscalas(List<TipoEscala> tipos, List<DiaSemana> escalaDia,
        List<Escala> escalaIntegrantes, List<Notification> erros)
    {
        foreach (var tipo in tipos)
        {
            switch (tipo)
            {
                case TipoEscala.Ministro:
                    {
                        var ministros = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Ministro);
                        if (ministros == null || !ministros.Any())
                            break;

                        var escalaMinistro = await GerarEscala(ministros, escalaDia, TipoEscala.Ministro);
                        escalaIntegrantes.AddRange(escalaMinistro);
                        break;
                    }

                case TipoEscala.BackingVocal:
                    {
                        var backingVocals =
                            await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.BackingVocal);
                        if (backingVocals == null || !backingVocals.Any())
                            break;

                        var escalaBackingVocal = await GerarEscala(backingVocals, escalaDia, TipoEscala.BackingVocal);
                        escalaIntegrantes.AddRange(escalaBackingVocal);
                        break;
                    }

                case TipoEscala.Teclado:
                    {
                        var tecladistas = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Teclado);
                        if (tecladistas == null || !tecladistas.Any())
                            break;

                        var escalaTeclado = await GerarEscala(tecladistas, escalaDia, TipoEscala.Teclado);
                        escalaIntegrantes.AddRange(escalaTeclado);
                        break;
                    }

                case TipoEscala.Violao:
                    {
                        var violonistas = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Violao);
                        if (violonistas == null || !violonistas.Any())
                            break;

                        var escalaViolao = await GerarEscala(violonistas, escalaDia, TipoEscala.Violao);
                        escalaIntegrantes.AddRange(escalaViolao);
                        break;
                    }

                case TipoEscala.ContraBaixo:
                    {
                        var baixistas = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.ContraBaixo);
                        if (baixistas == null || !baixistas.Any())
                            break;

                        var escalaBaixo = await GerarEscala(baixistas, escalaDia, TipoEscala.ContraBaixo);
                        escalaIntegrantes.AddRange(escalaBaixo);
                        break;
                    }

                case TipoEscala.Guitarra:
                    {
                        var guitarristas = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Guitarra);
                        if (guitarristas == null || !guitarristas.Any())
                            break;

                        var escalaGuitarra = await GerarEscala(guitarristas, escalaDia, TipoEscala.Guitarra);
                        escalaIntegrantes.AddRange(escalaGuitarra);
                        break;
                    }

                default:
                    erros.Add(new Notification("TipoIntegrante", $"Tipo {tipo} não reconhecido."));
                    break;
            }
        }
    }

    private async Task<List<Escala>> GerarEscala(List<Integrante> integrantes, List<DiaSemana> diasEscala,
        TipoEscala tipoEscala)
    {
        var escala = new List<Escala>();
        Integrante integranteEscolhido;
        var random = new Random();
        bool primeiroDiaSelecionado = false;

        var escalas = await _escalaRepository.ObterEscalas();
        var escalasObtidas = escalas.ParaListaEscala();

        foreach (var dia in diasEscala)
        {
            var disponiveis = integrantes.Where(i => i.DiasDaSemanaDisponiveis.Contains(dia.DayOfWeek)).ToList();

            if (disponiveis.Count <= 0)
            {
                Console.WriteLine($"Nenhum integrante disponível para {dia.Data}.");
                continue;
            }

            var escalaExistente = escalasObtidas.Any(x => x.Data == dia.Data.Date && x.TipoEscala == tipoEscala);

            if (escalaExistente)
                continue;

            // Para o primeiro dia
            if (escalasObtidas.Count <= 0 && !primeiroDiaSelecionado)
            {
                integranteEscolhido = disponiveis[random.Next(disponiveis.Count)];
                primeiroDiaSelecionado = true;
            }
            else
            {
                var contagemSelecoes = disponiveis.ToDictionary(
                    i => i,
                    i => escalasObtidas.Count(e =>
                        e.Integrante.IdIntegrante == i.IdIntegrante && e.TipoEscala == tipoEscala));

                var minSelecoes = contagemSelecoes.Values.Min();

                var menosSelecionados =
                    contagemSelecoes.Where(cs => cs.Value == minSelecoes).Select(cs => cs.Key).ToList();

                integranteEscolhido = menosSelecionados[random.Next(menosSelecionados.Count)];
            }

            escala.Add(new Data.Entities.Escala(
                integranteEscolhido,
                dia.Data,
                dia.DayOfWeek,
                tipoEscala));
        }

        return escala;
    }
}