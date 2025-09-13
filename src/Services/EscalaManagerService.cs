using System.Globalization;
using CsvHelper;
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

    public async Task<Result<EscalaResponse>> ObterEscalaPorId(int idEscala)
    {
        var erros = new List<Notification>();
        var escaladto = await _escalaRepository.ObterEscalaPorId(idEscala);

        if (escaladto == null)
        {
            erros.Add(new Notification(null, $"Escala não encontrada."));
            return Result<EscalaResponse>.NotFound(erros);
        }

        var escala = escaladto.ParaEscalaResultDto();

        return Result<EscalaResponse>.Ok(escala);
    }

    public async Task<Result<List<Escala>>> CriarEscala(EscalaIntegrantes escala)
    {
        List<Notification> erros = [];
        List<Escala> escalasVisualizacao = [];

        await ValidarErros(escala, erros);

        if (erros.Count != 0)
            return Result<List<Escala>>.BadRequest(erros);

        foreach (var dia in escala.DiasDaSemana)
        {
            List<Escala> escalaIntegrantes = [];
            var DiasDaEscala = ObterDiasEscala(escala.DataInicio, escala.DataFim, dia);

            foreach (var tipo in escala.TipoEscala)
            {
                var filtro = new IntegranteFiltro
                {
                    TipoIntegrante = tipo,
                    DiaDisponivel = (DayOfWeek)dia
                };

                var integranteDto = await _integranteRepository.ObterIntegrantes(filtro);
                if (integranteDto.integrantes == null || integranteDto.integrantes.Count == 0)
                    continue;

                var escalasExistentes = await _escalaRepository.ObterEscalas(new EscalaFiltro());
                var escalasObtidas = escalasExistentes.ParaListaEscala();

                foreach (var diaDaEscala in DiasDaEscala)
                {
                    var integrantes = integranteDto.integrantes.ParaIntegrantes();
                    Integrante integranteEscolhido;
                    var random = new Random();
                    bool primeiroDiaSelecionado = false;

                    if (escalasObtidas.Count(e => e.Data.Date == diaDaEscala.Data.Date && e.TipoEscala == tipo) > 0)
                        continue;

                    // Para o primeiro dia
                    if (escalasObtidas.Count <= 0 && !primeiroDiaSelecionado)
                    {
                        integranteEscolhido = integrantes[random.Next(integrantes.Count)];
                        primeiroDiaSelecionado = true;
                    }
                    else
                    {
                        var contagemSelecoes = integrantes.ToDictionary(
                            i => i,
                            i => escalasObtidas.Count(e =>
                                    e.Integrante.IdIntegrante == i.IdIntegrante && e.TipoEscala == tipo));

                        var minSelecoes = contagemSelecoes.Values.Min();

                        var menosSelecionados =
                            contagemSelecoes.Where(cs => cs.Value == minSelecoes).Select(cs => cs.Key).ToList();

                        integranteEscolhido = menosSelecionados[random.Next(menosSelecionados.Count)];
                    }

                    escalasObtidas.Add(new Escala(
                            integranteEscolhido,
                            diaDaEscala.Data.Date,
                            tipo));

                    escalaIntegrantes.Add(new Escala(
                        integranteEscolhido,
                        diaDaEscala.Data.Date,
                        tipo));
                }
            }

            var escalaDto = escalaIntegrantes.OrderByDescending(e => e.Data).ToList().ParaListaEscalaDto();

            if (!escala.Persistir)
            {
                escalasVisualizacao.AddRange(escalaIntegrantes);
                continue;
            }

            await _escalaRepository.InserirEscala(escalaDto);
        }

        if (!escala.Persistir)
        {
            return Result<List<Escala>>.Ok(escalasVisualizacao.OrderByDescending(e => e.Data).ToList());
        }

        var escalasCriadas = await _escalaRepository.ObterEscalas(new EscalaFiltro
        {
            Skip = 0,
            Take = 100,
            DataInicio = escala.DataInicio,
            DataFim = escala.DataFim
        });

        var escalasResult = escalasCriadas.ParaListaEscala();

        return Result<List<Escala>>.Created(escalasResult);
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

    public async Task<Result<List<EscalaResponse>>> ObterEscalas(EscalaFiltro escalaFiltro)
    {
        var erros = new List<Notification>();

        if (escalaFiltro.Skip < 0 || escalaFiltro.Take <= 0)
        {
            erros.Add(new Notification("Paginacao", "Os parâmetros de paginação devem ser maiores que zero."));
            return Result<List<EscalaResponse>>.BadRequest(erros);
        }

        if (escalaFiltro.DataInicio.HasValue && escalaFiltro.DataFim.HasValue && escalaFiltro.DataInicio > escalaFiltro.DataFim)
        {
            erros.Add(new Notification("Data", "A data de início não pode ser maior que a data de fim."));
            return Result<List<EscalaResponse>>.BadRequest(erros);
        }

        var escalasDto = await _escalaRepository.ObterEscalas(escalaFiltro);
        var escalas = escalasDto.ParaListaEscalaResultDto();

        return Result<List<EscalaResponse>>.Ok(escalas.OrderByDescending(e => e.Data.Value).ToList());
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
        escaladto.Data = escala.Data.Date;
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

    public async Task<Result<EscalaResponse>> ImportarEscalasDeCsv(IFormFile csvContent, bool substituirExistentes)
    {
        var erros = new List<Notification>();
        if (csvContent == null || csvContent.Length == 0)
        {
            erros.Add(new Notification("Arquivo", $"Arquivo não enviado."));
            return Result<EscalaResponse>.BadRequest(erros);
        }

        var anonymousTypeDefinition = new
        {
            Data = string.Empty,
            Ministro = string.Empty,
            BackingVocal = string.Empty,
            BackingVocal2 = string.Empty,
            Bateria = string.Empty,
            ContraBaixo = string.Empty,
            Teclado = string.Empty,
            Violao = string.Empty
        };

        using var stream = csvContent.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords(anonymousTypeDefinition).ToList();

        foreach (var record in records)
        {
            foreach (var prop in record.GetType().GetProperties())
            {
                if (prop.Name != "Data" && prop.GetValue(record)?.ToString() != "null")
                {
                    var data = DateTime.ParseExact(record.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    var tipo = prop.Name switch
                    {
                        "Ministro" => 1,
                        "BackingVocal" => 2,
                        "BackingVocal2" => 3,
                        "Teclado" => 4,
                        "Violao" => 5,
                        "ContraBaixo" => 6,
                        "Guitarra" => 7,
                        "Bateria" => 8,
                        _ => 0
                    };

                    var escalaExistente = await _escalaRepository.ObterEscalas(new EscalaFiltro
                    {
                        DataInicio = data,
                        DataFim = data,
                        Tipo = tipo
                    });

                    if (escalaExistente?.Count > 0 && substituirExistentes)
                    {
                        var escalaAtualizada = await _escalaRepository.AtualizarEscala(escalaExistente[0].IdEscala, new EscalaDto
                        {
                            IdEscala = escalaExistente[0].IdEscala,
                            Data = data,
                            TipoEscala = tipo,
                            IdIntegrante = escalaExistente[0].IdIntegrante
                        });
                    }
                    else if (escalaExistente == null || escalaExistente.Count == 0)
                    {
                        var (integrantes, total) = await _integranteRepository.ObterIntegrantes(new IntegranteFiltro
                        {
                            Nome = prop.GetValue(record)?.ToString(),
                            TipoIntegrante = tipo
                        });

                        if (integrantes?.Count > 0)
                        {
                            var novaEscala = new EscalaDto
                            {
                                Data = data,
                                TipoEscala = tipo,
                                IdIntegrante = integrantes[0].IdIntegrante
                            };

                            await _escalaRepository.InserirEscala(new List<EscalaDto> { novaEscala });
                        }
                    }
                }
            }
        }
        return Result<EscalaResponse>.Ok(new EscalaResponse());
    }
}