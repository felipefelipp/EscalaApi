using EscalaApi.Data.Entities;
using EscalaApi.Data.Repositories.Interfaces;
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

    public async Task<Result> CriarEscala(EscalaIntegrantes escala)
    {
        if (escala == null)
            return Result.BadRequest(new List<Notification>
                { new Notification("Escala", "Escala não encontrada.") });

        var escalaIntegrantes = new List<Data.Entities.Escala>();
        var erros = new List<Notification>();

        foreach (var dia in escala.DiasDaEscala)
        {
            var escalaDia = ObterDiasEscala(escala.DataInicio, escala.DataFim, dia);

            if (escala.TipoIntegrante is not null)
            {
                switch (escala.TipoIntegrante)
                {
                    case TipoIntegrante.Ministro:
                        var ministros = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Ministro);
                        if (ministros == null || !ministros.Any())
                        {
                            break;
                        }

                        var escalaMinistro = GerarEscala(ministros, escalaDia, TipoEscala.Ministro);

                        escalaIntegrantes.AddRange(escalaMinistro);
                        break;

                    case TipoIntegrante.BackingVocal:
                        var backingVocals =
                            await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.BackingVocal);
                        if (backingVocals == null || !backingVocals.Any())
                        {
                            break;
                        }

                        var escalaBackingVocal = GerarEscala(backingVocals, escalaDia, TipoEscala.BackingVocal);

                        escalaIntegrantes.AddRange(escalaBackingVocal);
                        break;

                    case TipoIntegrante.Instrumentista:
                        var instrumentistas =
                            await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Instrumentista);
                        if (instrumentistas == null || !instrumentistas.Any())
                        {
                            break;
                        }

                        var escalaInstrumentista = GerarEscala(instrumentistas, escalaDia, TipoEscala.Instrumentista);

                        escalaIntegrantes.AddRange(escalaInstrumentista);
                        break;

                    default:
                        erros.Add(new Notification("TipoIntegrante", $"Tipo {escala.TipoIntegrante} não reconhecido."));
                        break;
                }
            }
            else
            {
                var ministros = await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Ministro);
                if (ministros == null || !ministros.Any())
                {
                    var escalaMinistro = GerarEscala(ministros, escalaDia, TipoEscala.Ministro);

                    escalaIntegrantes.AddRange(escalaMinistro);
                }

                var backingVocals =
                    await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.BackingVocal);
                if (backingVocals == null || !backingVocals.Any())
                {
                    var escalaBackingVocal = GerarEscala(backingVocals, escalaDia, TipoEscala.BackingVocal);

                    escalaIntegrantes.AddRange(escalaBackingVocal);
                }

                var instrumentistas =
                    await _integranteRepository.ObterIntegrantesPorTipo(TipoIntegrante.Instrumentista);
                if (instrumentistas == null || !instrumentistas.Any())
                {
                    var escalaInstrumentista = GerarEscala(instrumentistas, escalaDia, TipoEscala.Instrumentista);

                    escalaIntegrantes.AddRange(escalaInstrumentista);
                }
            }
        }

        if (erros.Any())
            return Result.BadRequest(erros);

        await _escalaRepository.InserirEscala(escalaIntegrantes);

        return Result.Ok();
    }

    public async Task<Result<List<Data.Entities.Escala>>> ObterEscalas()
    {
        var escalas = await _escalaRepository.ObterEscalas();
        
        return Result<List<Data.Entities.Escala>>.Ok(escalas);
    }
    // public void ObterEscalaMinistros(EscalaIntegrantes escalaIntegrantes)
    // {
    //     var ministros = escalaIntegrantes.Integrantes.Where(i => i.TipoIntegrante == TipoIntegrante.Ministro).ToList();
    //     var backingVocal = escalaIntegrantes.Integrantes.Where(i => i.TipoIntegrante == TipoIntegrante.BackingVocal)
    //         .ToList();
    //     var instrumentista = escalaIntegrantes.Integrantes.Where(i => i.TipoIntegrante == TipoIntegrante.Instrumentista)
    //         .ToList();
    //
    //     var escala = new List<Data.Entities.Escala>();
    //
    //     foreach (var dia in escalaIntegrantes.Dias)
    //     {
    //         var escalaDia = ObterDiasEscala(escalaIntegrantes.DataInicio, escalaIntegrantes.DataFim, dia);
    //         var escalaMinistro = GerarEscala(ministros, escalaDia);
    //         var escalaBackingVocal = GerarEscalaBackingVocal(ministros, backingVocal, escalaDia);
    //         //var escalaInstrumentista = GerarEscala(instrumentista, escalaDia);
    //
    //         escala.AddRange(escalaMinistro);
    //         escala.AddRange(escalaBackingVocal);
    //         // escala.AddRange(escalaInstrumentista);
    //     }
    //
    //     var escalaOrdenada = escala.OrderBy(e => e.Data).ToList();
    //     //var escalaAjustada = AjustarIntegranteSubsequenteComHistorico(escalaOrdenada);
    //     //var ajusteEscalaMinistro = AjustarIntegranteMinistroSubsequenteComHistorico(escalaOrdenada);
    //     //var ajusteEscalaBackingVocal = AjustarIntegranteBackingVocalSubsequenteComHistorico(ajusteEscalaMinistro);
    //     ImprimirEscala.ExportarEscala(escalaOrdenada, escalaIntegrantes.DataInicio, escalaIntegrantes.DataFim);
    // }

    // private static List<Escala> AjustarIntegranteSubsequenteComHistorico(List<Escala> escala)
    // {
    //     // Dicionário para contar as ocorrências de cada integrante na escala
    //     var contadorSelecoes = escala
    //         .GroupBy(e => e.Integrante.Nome)
    //         .ToDictionary(g => g.Key, g => g.Count());
    //
    //     for (int i = 1; i < escala.Count; i++)
    //     {
    //         // Se o integrante atual for igual ao anterior
    //         if (escala[i].Integrante.Nome == escala[i - 1].Integrante.Nome)
    //         {
    //             bool encontrouSubstituto = false;
    //
    //             // Busca o integrante menos selecionado que atende às condições
    //             var candidato = escala
    //                 .Where(e =>
    //                     e.NomeSemana == escala[i].NomeSemana && // Mesmo dia da semana
    //                     e.Integrante.TipoIntegrante == escala[i].Integrante.TipoIntegrante && // Mesmo tipo de integrante
    //                     e.Integrante.Nome != escala[i].Integrante.Nome && // Diferente do atual
    //                     e.Integrante.Nome != escala[i - 1].Integrante.Nome && // Diferente do anterior
    //                     (i + 1 >= escala.Count || e.Integrante.Nome != escala[i + 1].Integrante.Nome) && // Diferente do próximo
    //                     !escala.Skip(Math.Max(0, i - 3)).Take(3).Any(x => x.Integrante.Nome == e.Integrante.Nome)
    //                     ) // Não foi usado recentemente
    //                 .OrderBy(e => contadorSelecoes[e.Integrante.Nome]) // Ordena pelo menor número de seleções
    //                 .FirstOrDefault();
    //
    //             if (candidato != null)
    //             {
    //                 // Realiza a troca
    //                 (escala[i].Integrante, candidato.Integrante) = (candidato.Integrante, escala[i].Integrante);
    //
    //                 // Atualiza o contador de seleções
    //                 contadorSelecoes[escala[i].Integrante.Nome]++;
    //                 contadorSelecoes[candidato.Integrante.Nome]--;
    //
    //                 encontrouSubstituto = true;
    //             }
    //
    //             if (!encontrouSubstituto)
    //             {
    //                 Console.WriteLine($"Não foi possível ajustar o integrante {escala[i].Integrante.Nome} na posição {i}.");
    //             }
    //         }
    //     }
    //
    //     return escala;
    // }
    //
    // #region ajusteEscala
    //
    // private static List<Escala> AjustarIntegranteBackingVocalSubsequenteComHistorico(List<Escala> escala)
    // {
    //     // Dicionário para contar as ocorrências de cada integrante na escala
    //     var contadorSelecoes = escala
    //         .GroupBy(e => e.Integrante.Nome)
    //         .ToDictionary(g => g.Key, g => g.Count());
    //
    //     for (int i = 1; i < escala.Count; i++)
    //     {
    //         // Se o integrante atual for igual ao anterior
    //         if (escala[i].Integrante.Nome == escala[i - 1].Integrante.Nome && escala[i].Integrante.TipoIntegrante == TipoIntegrante.BackingVocal)
    //         {
    //             bool encontrouSubstituto = false;
    //
    //             // Busca o integrante menos selecionado que atende às condições
    //             var candidato = escala
    //                 .Where(e =>
    //                     e.NomeSemana == escala[i].NomeSemana && // Mesmo dia da semana
    //                     e.Integrante.TipoIntegrante == escala[i].Integrante.TipoIntegrante && // Mesmo tipo de integrante
    //                     e.Integrante.Nome != escala[i].Integrante.Nome && // Diferente do atual
    //                     e.Integrante.Nome != escala[i - 1].Integrante.Nome && // Diferente do anterior
    //                     (i + 1 >= escala.Count || e.Integrante.Nome != escala[i + 1].Integrante.Nome) && // Diferente do próximo
    //                     !escala.Skip(Math.Max(0, i - 3)).Take(3).Any(x => x.Integrante.Nome == e.Integrante.Nome)
    //                     ) // Não foi usado recentemente
    //                 .OrderBy(e => contadorSelecoes[e.Integrante.Nome]) // Ordena pelo menor número de seleções
    //                 .FirstOrDefault();
    //
    //             if (candidato != null)
    //             {
    //                 // Realiza a troca
    //                 (escala[i].Integrante, candidato.Integrante) = (candidato.Integrante, escala[i].Integrante);
    //
    //                 // Atualiza o contador de seleções
    //                 contadorSelecoes[escala[i].Integrante.Nome]++;
    //                 contadorSelecoes[candidato.Integrante.Nome]--;
    //
    //                 encontrouSubstituto = true;
    //             }
    //
    //             if (!encontrouSubstituto)
    //             {
    //                 Console.WriteLine($"Não foi possível ajustar o integrante {escala[i].Integrante.Nome} na posição {i}.");
    //             }
    //         }
    //     }
    //
    //     return escala;
    // }
    //
    // private static List<Escala> AjustarIntegranteMinistroSubsequenteComHistorico(List<Escala> escala)
    // {
    //     // Dicionário para contar as ocorrências de cada integrante na escala
    //     var contadorSelecoes = escala
    //         .GroupBy(e => e.Integrante.Nome)
    //         .ToDictionary(g => g.Key, g => g.Count());
    //
    //     for (int i = 1; i < escala.Count; i++)
    //     {
    //         // Se o integrante atual for igual ao anterior
    //         if (escala[i].Integrante.Nome == escala[i - 1].Integrante.Nome && escala[i].Integrante.TipoIntegrante == TipoIntegrante.Ministro)
    //         {
    //             bool encontrouSubstituto = false;
    //
    //             // Busca o integrante menos selecionado que atende às condições
    //             var candidato = escala
    //                 .Where(e =>
    //                     e.NomeSemana == escala[i].NomeSemana && // Mesmo dia da semana
    //                     e.Integrante.TipoIntegrante == escala[i].Integrante.TipoIntegrante && // Mesmo tipo de integrante
    //                                                                                           //escala[i].Integrante.TipoIntegrante == TipoIntegrante.Ministro &&
    //                     e.Integrante.Nome != escala[i].Integrante.Nome && // Diferente do atual
    //                     e.Integrante.Nome != escala[i - 1].Integrante.Nome && // Diferente do anterior
    //                     (i + 1 >= escala.Count || e.Integrante.Nome != escala[i + 1].Integrante.Nome) && // Diferente do próximo
    //                     !escala.Skip(Math.Max(0, i - 3)).Take(3).Any(x => x.Integrante.Nome == e.Integrante.Nome)
    //                     ) // Não foi usado recentemente
    //                 .OrderBy(e => contadorSelecoes[e.Integrante.Nome]) // Ordena pelo menor número de seleções
    //                 .FirstOrDefault();
    //
    //             if (candidato != null)
    //             {
    //                 // Realiza a troca
    //                 (escala[i].Integrante, candidato.Integrante) = (candidato.Integrante, escala[i].Integrante);
    //
    //                 // Atualiza o contador de seleções
    //                 contadorSelecoes[escala[i].Integrante.Nome]++;
    //                 contadorSelecoes[candidato.Integrante.Nome]--;
    //
    //                 encontrouSubstituto = true;
    //             }
    //
    //             if (!encontrouSubstituto)
    //             {
    //                 Console.WriteLine($"Não foi possível ajustar o integrante {escala[i].Integrante.Nome} na posição {i}.");
    //             }
    //         }
    //     }
    //
    //     return escala;
    // }
    //
    // #endregion

    private static List<Data.Entities.Escala> GerarEscala(List<Integrante> integrantes,
        List<DiaSemana> diasEscala,
        TipoEscala tipoEscala)
    {
        var escala = new List<Data.Entities.Escala>();
        Integrante? primeiroEscolhido = null;
        Integrante integranteEscolhido;
        var random = new Random();
        var contadorEscala = integrantes.ToDictionary(i => i, i => 0);
        bool primeiroDiaSelecionado = false;

        foreach (var dia in diasEscala)
        {
            var disponiveis = integrantes.Where(i => i.DiasDisponiveis.Contains(dia.DayOfWeek)).ToList();

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

    // private static List<Data.Entities.Escala> GerarEscalaBackingVocal(List<Integrante> ministros,
    //     List<Integrante> integrantes,
    //     List<DiaSemana> diasEscala)
    // {
    //     var escala = new List<Data.Entities.Escala>();
    //     var random = new Random();
    //     var contadorEscala = new int[integrantes.Count]; // Contador baseado em índices
    //     int? ultimoIndiceEscolhido = null;
    //
    //     foreach (var dia in diasEscala)
    //     {
    //         // Filtrar os índices dos integrantes disponíveis
    //         var indicesDisponiveis = integrantes
    //             .Select((integrante, index) => new { Integrante = integrante, Index = index })
    //             .Where(x => x.Integrante.DiasDisponiveis.Contains(dia.DayOfWeek))
    //             .Select(x => x.Index)
    //             .ToList();
    //
    //         if (indicesDisponiveis.Count <= 0)
    //         {
    //             Console.WriteLine($"Nenhum integrante disponível para {dia.Data}.");
    //             continue;
    //         }
    //
    //         int indiceEscolhido;
    //         do
    //         {
    //             // Sorteia aleatoriamente um índice disponível
    //             indiceEscolhido = indicesDisponiveis[random.Next(indicesDisponiveis.Count)];
    //         } while (
    //             // Reapete o sorteio se o índice pertence a um ministro
    //             ministros.Select(min => integrantes.IndexOf(min)).Contains(indiceEscolhido) ||
    //             // Ou se o índice é consecutivo ou igual ao de um ministro
    //             ministros.Select(min => integrantes.IndexOf(min)).Any(indiceMinistro =>
    //                 Math.Abs(indiceEscolhido - indiceMinistro) <= 1) ||
    //             // Ou se o índice é o mesmo do último escolhido
    //             indiceEscolhido == ultimoIndiceEscolhido
    //         );
    //
    //         // Atualiza o último índice escolhido
    //         ultimoIndiceEscolhido = indiceEscolhido;
    //
    //         // Incrementa o contador do integrante pelo índice
    //         contadorEscala[indiceEscolhido]++;
    //
    //         // Adiciona à escala usando o índice
    //         escala.Add(new Data.Entities.Escala(
    //             integrantes[indiceEscolhido],
    //             dia.Data,
    //             dia.DayOfWeek));
    //     }
    //
    //     return escala;
    // }

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
}