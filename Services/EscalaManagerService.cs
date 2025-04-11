using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
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

    public async Task<Result<List<Escala>>> CriarEscala(EscalaIntegrantes escala)
    {
        var escalaDto = new List<EscalaDto>();
        var escalaIntegrantes = new List<Escala>();
        var erros = new List<Notification>();

        foreach (var dia in escala.DiasDaSemana)
        {
            var DiasDaEscala = ObterDiasEscala(escala.DataInicio, escala.DataFim, dia);

            if (escala.TipoIntegrante is not null)
            {
                ProcessarEscalas(escala.TipoIntegrante, DiasDaEscala, escalaIntegrantes, erros);
            }
        }

        if (erros.Any())
            return Result<List<Escala>>.BadRequest(erros);

        foreach (var escalaIntegrante in escalaIntegrantes)
        {
            escalaDto.Add(new EscalaDto(escalaIntegrante.Integrante.IdIntegrante,
                escalaIntegrante.Data,
                (int)escalaIntegrante.TipoEscala));
        }

        //await _escalaRepository.InserirEscala(escalaDto);
        return Result<List<Escala>>.Ok(escalaIntegrantes);
    }

    public async Task<Result<List<Escala>>> ObterEscalas()
    {
        var escalas = await _escalaRepository.ObterEscalas();

        return Result<List<Escala>>.Ok(escalas);
    }

    private static List<Escala> GerarEscala(List<Integrante> integrantes,
        List<DiaSemana> diasEscala,
        TipoEscala tipoEscala)
    {
        var escala = new List<Escala>();
        Integrante? primeiroEscolhido = null;
        Integrante integranteEscolhido;
        var random = new Random();
        var contadorEscala = integrantes.ToDictionary(i => i, i => 0);
        bool primeiroDiaSelecionado = false;

        foreach (var dia in diasEscala)
        {
            var disponiveis = integrantes.Where(i => i.DiasDaSemanaDisponiveis.Contains(dia.DayOfWeek)).ToList();

            if (disponiveis.Count <= 0)
            {
                Console.WriteLine($"Nenhum integrante disponível para {dia.Data}.");
                continue;
            }

            // Para o primeiro dia
            if (!primeiroDiaSelecionado)
            {
                integranteEscolhido = disponiveis[random.Next(disponiveis.Count)];
                primeiroEscolhido = integranteEscolhido;
                contadorEscala[integranteEscolhido]++;
                primeiroDiaSelecionado = true;
            }
            else
            {
                integranteEscolhido = disponiveis
                    .OrderBy(i => contadorEscala[i]) // Ordena pelos menos escalados
                    .First();

                contadorEscala[integranteEscolhido]++;
            }

            escala.Add(new Data.Entities.Escala(
                integranteEscolhido,
                dia.Data,
                dia.DayOfWeek,
                tipoEscala));
        }

        return escala;
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

    private async Task ProcessarEscalas(
        List<TipoIntegrante> tipos,
        List<DiaSemana> escalaDia,
        List<Escala> escalaIntegrantes,
        List<Notification> erros)
    {
        foreach (var tipo in tipos)
        {
            switch (tipo)
            {
                case TipoIntegrante.Ministro:
                {
                    var ministros = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
                    if (ministros == null || !ministros.Any())
                        break;

                    var escalaMinistro = GerarEscala(ministros, escalaDia, TipoEscala.Ministro);
                    escalaIntegrantes.AddRange(escalaMinistro);
                    break;
                }

                case TipoIntegrante.BackingVocal:
                {
                    var backingVocals = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
                    if (backingVocals == null || !backingVocals.Any())
                        break;

                    var escalaBackingVocal = GerarEscala(backingVocals, escalaDia, TipoEscala.BackingVocal);
                    escalaIntegrantes.AddRange(escalaBackingVocal);
                    break;
                }

                case TipoIntegrante.Teclado:
                {
                    var tecladistas = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
                    if (tecladistas == null || !tecladistas.Any())
                        break;

                    var escalaTeclado = GerarEscala(tecladistas, escalaDia, TipoEscala.Teclado);
                    escalaIntegrantes.AddRange(escalaTeclado);
                    break;
                }

                case TipoIntegrante.Violao:
                {
                    var violonistas = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
                    if (violonistas == null || !violonistas.Any())
                        break;

                    var escalaViolao = GerarEscala(violonistas, escalaDia, TipoEscala.Violao);
                    escalaIntegrantes.AddRange(escalaViolao);
                    break;
                }

                case TipoIntegrante.ContraBaixo:
                {
                    var baixistas = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
                    if (baixistas == null || !baixistas.Any())
                        break;

                    var escalaBaixo = GerarEscala(baixistas, escalaDia, TipoEscala.ContraBaixo);
                    escalaIntegrantes.AddRange(escalaBaixo);
                    break;
                }

                case TipoIntegrante.Guitarra:
                {
                    var guitarristas = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
                    if (guitarristas == null || !guitarristas.Any())
                        break;

                    var escalaGuitarra = GerarEscala(guitarristas, escalaDia, TipoEscala.Guitarra);
                    escalaIntegrantes.AddRange(escalaGuitarra);
                    break;
                }

                default:
                    erros.Add(new Notification("TipoIntegrante", $"Tipo {tipo} não reconhecido."));
                    break;
            }
        }
    }

    private TipoEscala? MapearTipoEscala(TipoIntegrante tipo)
    {
        return tipo switch
        {
            TipoIntegrante.Ministro => TipoEscala.Ministro,
            TipoIntegrante.BackingVocal => TipoEscala.BackingVocal,
            TipoIntegrante.Teclado => TipoEscala.Teclado,
            TipoIntegrante.Violao => TipoEscala.Violao,
            TipoIntegrante.ContraBaixo => TipoEscala.ContraBaixo,
            TipoIntegrante.Guitarra => TipoEscala.Guitarra,
            _ => null
        };
    }
}