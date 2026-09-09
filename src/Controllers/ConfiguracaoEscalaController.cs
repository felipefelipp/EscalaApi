using System.Net;
using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Molde de geração: período, dias, tipos e estratégia. Entrada de POST /escalas/gerar.</summary>
[ApiController]
[Tags("Configuração de escala")]
public class ConfiguracaoEscalaController : ControllerBase
{
    private readonly IConfiguracaoEscalaService _service;

    public ConfiguracaoEscalaController(IConfiguracaoEscalaService service)
    {
        _service = service;
    }

    /// <summary>Lista todas as configurações cadastradas.</summary>
    [HttpGet("/configuracoes-escala")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    /// <summary>Obtém uma configuração pelo ID, incluindo slots e tipos vinculados.</summary>
    [HttpGet("/configuracoes-escala/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    /// <summary>
    /// Lista as datas concretas geradas a partir de dataInicio/dataFim e valoresRecorrentes.
    /// Útil para validar o molde antes de POST /escalas/gerar.
    /// </summary>
    [HttpGet("/configuracoes-escala/{id}/datas-expandidas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DatasExpandidas(int id)
    {
        var retorno = await _service.ObterDatasExpandidasAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    /// <summary>
    /// Cria o molde de geração: período, dias recorrentes e tipos.
    /// Use o ID retornado em POST /escalas/gerar.
    /// </summary>
    /// <remarks>
    /// **Campos principais**
    /// - `valoresRecorrentes`: dias da semana no formato DayOfWeek — 0=Dom, 1=Seg, 2=Ter, 3=Qua, 4=Qui, 5=Sex, 6=Sáb.
    ///   Ex.: `[3, 0]` gera escala só em quartas e domingos entre `dataInicio` e `dataFim`.
    /// - `tiposIntegrante`: papéis incluídos na rotação (IDs de GET /tipos-integrante).
    ///
    /// **Validações (422)**
    /// - Nome obrigatório; `dataInicio` ≤ `dataFim`
    /// - Pelo menos um valor em `valoresRecorrentes` e em `tiposIntegrante`
    /// - Intervalo dentro do limite de GET /parametros (chave `range_maximo_escala`)
    ///
    /// **Exemplo**
    /// ```json
    /// {
    ///   "nome": "Q1 2026",
    ///   "dataInicio": "2026-01-01",
    ///   "dataFim": "2026-03-31",
    ///   "valoresRecorrentes": [3, 0],
    ///   "tiposIntegrante": [1, 2]
    /// }
    /// ```
    /// </remarks>
    [HttpPost("/configuracoes-escala")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Inserir(ConfiguracaoEscalaRequest request)
    {
        var retorno = await _service.InserirAsync(request);
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.UnprocessableEntity)
                return UnprocessableEntity(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return StatusCode((int)HttpStatusCode.Created, retorno.Object);
    }

    /// <summary>Atualiza configuração. Após primeira persistência, estratégia fica imutável (409).</summary>
    [HttpPut("/configuracoes-escala/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Atualizar(int id, ConfiguracaoEscalaRequest request)
    {
        var retorno = await _service.AtualizarAsync(id, request);
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.Conflict)
                return Conflict(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.UnprocessableEntity)
                return UnprocessableEntity(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return Ok(retorno.Object);
    }
}
