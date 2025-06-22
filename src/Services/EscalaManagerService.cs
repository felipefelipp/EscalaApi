using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class EscalaManager : IEscalaManagerService
{
    public IIntegranteRepository _integranteRepository;
    public IEscalaRepository _escalaRepository;
    public ITipoEscalaRepository _tipoEscalaRepository;

    public EscalaManager(IIntegranteRepository integranteRepository,
                         IEscalaRepository escalaRepository,
                         ITipoEscalaRepository tipoEscalaRepository)
    {
        _integranteRepository = integranteRepository;
        _escalaRepository = escalaRepository;
        _tipoEscalaRepository = tipoEscalaRepository;
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
        List<Notification> erros = [];
        List<Escala> escalaIntegrantes = [];

        await ValidarErros(escala, erros);

        if (erros.Count != 0)
            return Result<List<Escala>>.BadRequest(erros);

        foreach (var dia in escala.DiasDaSemana)
        {
            var DiasDaEscala = ObterDiasEscala(escala.DataInicio.Date, escala.DataFim.Date, dia);

            await ProcessarEscalas(escala.TipoEscala, DiasDaEscala, escalaIntegrantes);
        }

        var escalaDto = escalaIntegrantes.ParaListaEscalaDto();

        await _escalaRepository.InserirEscala(escalaDto);

        return Result<List<Escala>>.Ok(escalaIntegrantes);
    }

    private async Task ValidarErros(EscalaIntegrantes escala, List<Notification> erros)
    {
        if (escala.DataInicio == default || escala.DataFim == default)
        {
            erros.Add(new Notification("Data", "Data de início e fim são obrigatórias."));
        }
        if (escala.DataInicio > escala.DataFim)
        {
            erros.Add(new Notification("Data", "A data de início não pode ser maior que a data de fim."));
        }
        if (escala.TipoEscala == null || escala.TipoEscala.Count == 0)
        {
            erros.Add(new Notification("TipoEscala", "Pelo menos um tipo de escala deve ser selecionado."));
        }
        if (escala.DiasDaSemana == null || escala.DiasDaSemana.Count == 0)
        {
            erros.Add(new Notification("DiasDaSemana", "Pelo menos um dia da semana deve ser selecionado."));
        }
        if (escala.DiasDaSemana is null)
        {
            erros.Add(new Notification("DiasDaSemana", "Pelo menos um dia da semana deve ser selecionado."));
        }
        if (escala.DiasDaSemana!.Count > 7)
        {
            erros.Add(new Notification("DiasDaSemana", "Não é possível selecionar mais de 7 dias da semana."));
        }
        if (escala.DiasDaSemana.Distinct().Count() != escala.DiasDaSemana.Count)
        {
            erros.Add(new Notification("DiasDaSemana", "Dias da semana duplicados não são permitidos."));
        }
        if (escala.DataInicio > escala.DataFim)
        {
            erros.Add(new Notification("Data", "A data de início não pode ser maior que a data de fim."));
        }
        if (escala.DataInicio == escala.DataFim && escala.DiasDaSemana.Count > 1)
        {
            erros.Add(new Notification("DiasDaSemana", "Não é possível selecionar mais de um dia da semana para a mesma data."));
        }
        if (escala.DataInicio == escala.DataFim && escala.TipoEscala!.Count == 0)
        {
            erros.Add(new Notification("TipoEscala", "Pelo menos um tipo de escala deve ser selecionado para a mesma data."));
        }
        if (escala.DataInicio == escala.DataFim && escala.DiasDaSemana.Count == 0)
        {
            erros.Add(new Notification("DiasDaSemana", "Pelo menos um dia da semana deve ser selecionado para a mesma data."));
        }
        var tipoEscalasDisponiveis = await _tipoEscalaRepository.ObterTiposEscalaDisponiveis();

        if (tipoEscalasDisponiveis == null || tipoEscalasDisponiveis.Count == 0)
        {
            erros.Add(new Notification("TipoEscala", "Nenhum tipo de escala disponível."));
        }

        if (escala.TipoEscala!.Any(tipo => !tipoEscalasDisponiveis!.Contains(tipo)))
        {
            erros.Add(new Notification("TipoEscala", "Um ou mais tipos de escala selecionados não estão disponíveis."));
        }
    }

    public async Task<Result<List<EscalaResultDto>>> ObterEscalas(EscalaFiltro escalaFiltro)
    {
        var erros = new List<Notification>();

        if (escalaFiltro.Skip < 0 || escalaFiltro.Take <= 0)
        {
            erros.Add(new Notification("Paginacao", "Os parâmetros de paginação devem ser maiores que zero."));
            return Result<List<EscalaResultDto>>.BadRequest(erros);
        }

        if (escalaFiltro.DataInicio.HasValue && escalaFiltro.DataFim.HasValue && escalaFiltro.DataInicio > escalaFiltro.DataFim)
        {
            erros.Add(new Notification("Data", "A data de início não pode ser maior que a data de fim."));
            return Result<List<EscalaResultDto>>.BadRequest(erros);
        }

        var escalasDto = await _escalaRepository.ObterEscalas(escalaFiltro);
        var escalas = escalasDto.ParaListaEscalaDto();

        return Result<List<EscalaResultDto>>.Ok(escalas);
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

    private async Task ProcessarEscalas(List<int> tipos,
                                        List<DiaSemana> escalaDia,
                                        List<Escala> escalaIntegrantes)
    {
        foreach (var tipo in tipos)
        {
            var integranteDto = await _integranteRepository.ObterIntegrantesPorTipo(tipo);
            if (integranteDto == null || integranteDto.Count == 0)
                break;

            var integrante = integranteDto.ParaIntegrantes();
            var escalaMinistro = await GerarEscala(integrante, escalaDia, tipo);
            escalaIntegrantes.AddRange(escalaMinistro);
            break;
        }
    }

    private async Task<List<Escala>> GerarEscala(List<Integrante> integrantes,
                                                 List<DiaSemana> diasEscala,
                                                 int tipoEscala)
    {
        var escala = new List<Escala>();
        Integrante integranteEscolhido;
        var random = new Random();
        bool primeiroDiaSelecionado = false;

        foreach (var dia in diasEscala)
        {
            var disponiveis = integrantes.Where(i => i.DiasDaSemanaDisponiveis.Contains(dia.DayOfWeek)).ToList();

            if (disponiveis.Count <= 0)
            {
                Console.WriteLine($"Nenhum integrante disponível para {dia.Data}.");
                continue;
            }

            var escalasExistentes = await _escalaRepository.ObterEscalas(new EscalaFiltro
            {
                DataInicio = dia.Data.Date,
                DataFim = dia.Data.Date,
                Tipo = tipoEscala
            });

            var escalasObtidas = escalasExistentes.ParaListaEscala();
            // var escalaExistente = escalasObtidas.Any(x => x.Data.Date == dia.Data.Date && x.TipoEscala == tipoEscala);

            if (escalasObtidas.Any())
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

            escala.Add(new Escala(
                integranteEscolhido,
                dia.Data,
                tipoEscala));
        }

        return escala;
    }
}