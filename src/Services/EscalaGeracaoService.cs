using EscalaApi.Data.Entities;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using EscalaApi.Services.Rotacao;
using EscalaApi.Services.Rotacao.Models;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class EscalaGeracaoService : IEscalaGeracaoService
{
    private readonly IConfiguracaoEscalaRepository _configuracaoRepository;
    private readonly IIntegranteRepository _integranteRepository;
    private readonly IEscalaRepository _escalaRepository;
    private readonly GeradorDePreview _geradorDePreview;

    public EscalaGeracaoService(
        IConfiguracaoEscalaRepository configuracaoRepository,
        IIntegranteRepository integranteRepository,
        IEscalaRepository escalaRepository,
        GeradorDePreview geradorDePreview)
    {
        _configuracaoRepository = configuracaoRepository;
        _integranteRepository = integranteRepository;
        _escalaRepository = escalaRepository;
        _geradorDePreview = geradorDePreview;
    }

    public async Task<Result<ResultadoPreview>> GerarPreviewAsync(GerarEscalaRequest request)
    {
        var config = await _configuracaoRepository.ObterPorIdAsync(request.ConfiguracaoEscalaId);
        if (config is null)
            return Result<ResultadoPreview>.NotFound([new Notification("ConfiguracaoEscalaId", "Configuração não encontrada.")]);

        var integrantes = new List<Integrante>();
        foreach (var tipo in config.TiposIntegrante)
        {
            var (dtos, _) = await _integranteRepository.ObterIntegrantes(new IntegranteFiltro { TipoIntegrante = tipo, Take = 1000 });
            integrantes.AddRange(dtos.ParaIntegrantes());
        }

        integrantes = integrantes
            .GroupBy(i => i.IdIntegrante)
            .Select(g => g.First())
            .ToList();

        var historicoDto = await _escalaRepository.ObterEscalas(new EscalaFiltro
        {
            IdConfiguracao = config.IdConfiguracao,
            Take = 10000
        });
        var historico = historicoDto.ParaListaEscala();

        var parametros = new ParametrosGeracaoPreview
        {
            ConfiguracaoEscalaId = config.IdConfiguracao,
            DataInicio = config.DataInicio,
            DataFim = config.DataFim,
            DiasDaSemana = config.ValoresRecorrentes.Select(v => (DayOfWeek)v).ToList(),
            TiposIntegrante = config.TiposIntegrante,
            Integrantes = integrantes,
            Historico = historico,
            CodigoEstrategia = "contextual_dia_semana",
            ImpedirMultiplosTiposMesmoDia = request.ImpedirMultiplosTiposMesmoDia
        };

        var resultado = await _geradorDePreview.GerarAsync(parametros);

        if (request.Persistir && resultado.Escalas.Count > 0)
        {
            var dtos = resultado.Escalas.ParaListaEscalaDto();
            foreach (var dto in dtos)
                dto.IdConfiguracao = config.IdConfiguracao;

            await _escalaRepository.InserirEscala(dtos);
        }

        return Result<ResultadoPreview>.Ok(resultado);
    }
}
