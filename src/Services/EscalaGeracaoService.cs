using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
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
    private readonly IParametroSistemaRepository _parametroRepository;
    private readonly GeradorDePreview _geradorDePreview;
    private readonly PersistidorDePreview _persistidorDePreview;

    public EscalaGeracaoService(
        IConfiguracaoEscalaRepository configuracaoRepository,
        IIntegranteRepository integranteRepository,
        IEscalaRepository escalaRepository,
        IParametroSistemaRepository parametroRepository,
        GeradorDePreview geradorDePreview,
        PersistidorDePreview persistidorDePreview)
    {
        _configuracaoRepository = configuracaoRepository;
        _integranteRepository = integranteRepository;
        _escalaRepository = escalaRepository;
        _parametroRepository = parametroRepository;
        _geradorDePreview = geradorDePreview;
        _persistidorDePreview = persistidorDePreview;
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

        var paramExp = await _parametroRepository.ObterPorChaveAsync("preview_expiracao_horas");
        var horasExp = int.TryParse(paramExp?.Valor, out var h) ? h : 24;

        var parametros = new ParametrosGeracaoPreview
        {
            ConfiguracaoEscalaId = config.IdConfiguracao,
            DataInicio = config.DataInicio,
            DataFim = config.DataFim,
            DiasDaSemana = config.ValoresRecorrentes.Select(v => (DayOfWeek)v).ToList(),
            TiposIntegrante = config.TiposIntegrante,
            Integrantes = integrantes,
            Historico = historico,
            CodigoEstrategia = config.CodigoEstrategia,
            ImpedirMultiplosTiposMesmoDia = request.ImpedirMultiplosTiposMesmoDia,
            HorasExpiracaoPreview = horasExp
        };

        var resultado = await _geradorDePreview.GerarAsync(parametros);
        return Result<ResultadoPreview>.Ok(resultado);
    }

    public async Task<Result<PersistenciaPreviewResultado>> PersistirPreviewAsync(string token)
    {
        var resultado = await _persistidorDePreview.PersistirAsync(new PreviewPersistRequest { PreviewToken = token });

        if (!resultado.Sucesso)
        {
            var msg = resultado.Mensagem ?? "Erro ao persistir preview.";
            if (msg.Contains("expirou", StringComparison.OrdinalIgnoreCase))
                return Result<PersistenciaPreviewResultado>.Gone([new Notification("TokenExpirado", msg)]);
            if (msg.Contains("já foi persistido", StringComparison.OrdinalIgnoreCase))
                return Result<PersistenciaPreviewResultado>.Gone([new Notification("JaPersistido", msg)]);
            if (msg.Contains("não encontrado", StringComparison.OrdinalIgnoreCase))
                return Result<PersistenciaPreviewResultado>.NotFound([new Notification("TokenInvalido", msg)]);
            return Result<PersistenciaPreviewResultado>.BadRequest([new Notification("Preview", msg)]);
        }

        return Result<PersistenciaPreviewResultado>.Created(resultado);
    }
}
