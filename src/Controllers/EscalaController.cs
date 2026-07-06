using System.Net;
using EscalaApi.Data.Entities;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using EscalaApi.Services.Rotacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Consulta, edição manual, geração automática e importação de escalas.</summary>
[ApiController]
[Route("[controller]")]
[Tags("Escalas")]
public class EscalaController : ControllerBase
{
    private readonly IEscalaManagerService _escalaManagerService;
    private readonly IEscalaGeracaoService _escalaGeracaoService;

    public EscalaController(IEscalaManagerService escalaManagerService, IEscalaGeracaoService escalaGeracaoService)
    {
        _escalaManagerService = escalaManagerService;
        _escalaGeracaoService = escalaGeracaoService;
    }

    /// <summary>Obtém uma escala persistida pelo ID, com integrantes alocados em cada tipo.</summary>
    [HttpGet("/escalas/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditarEscala(int id)
    {
        var retorno = await _escalaManagerService.ObterEscalaPorId(id);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Gera escala com algoritmo legado (monolítico). Prefira <c>POST /escalas/gerar</c>.</summary>
    [HttpPost("/escalas")]
    [Obsolete("Use POST /escalas/gerar")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarEscala(EscalaIntegrantes escala)
    {
        var retorno = await _escalaManagerService.CriarEscala(escala);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Executa o algoritmo e retorna preview com token. Não grava — use persistir para confirmar.</summary>
    [HttpPost("/escalas/gerar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GerarEscala(GerarEscalaRequest request)
    {
        var retorno = await _escalaGeracaoService.GerarPreviewAsync(request);
        if (!retorno.Sucess)
            return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    /// <summary>Grava o lote do preview. Token expira por TTL; após persistir, estratégia fica imutável.</summary>
    [HttpPost("/escalas/preview/{token}/persistir")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status410Gone)]
    public async Task<IActionResult> PersistirPreview(string token)
    {
        var retorno = await _escalaGeracaoService.PersistirPreviewAsync(token);
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.Gone)
                return StatusCode(410, new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return StatusCode((int)HttpStatusCode.Created, retorno.Object);
    }

    /// <summary>Lista escalas persistidas. Aceita filtros por período, tipo e integrante via query string.</summary>
    [HttpGet("/escalas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterEscalas([FromQuery] EscalaFiltro escalaFiltro)
    {
        var retorno = await _escalaManagerService.ObterEscalas(escalaFiltro);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Altera manualmente os integrantes de um slot de escala já persistido.</summary>
    [HttpPut("/escalas/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditarEscala(int id, EscalaIntegrante escala)
    {
        var retorno = await _escalaManagerService.EditarEscala(id, escala);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Importa escalas em lote via CSV. Colunas = nomes dos tipos no catálogo.</summary>
    /// <param name="file">Arquivo CSV com coluna de data e uma coluna por tipo.</param>
    /// <param name="substituirExistentes">Se true, sobrescreve slots já existentes na mesma data.</param>
    [HttpPost("/escalas/import-csv")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportCsv(IFormFile file, bool substituirExistentes = false)
    {
        var result = await _escalaManagerService.ImportarEscalasDeCsv(file, substituirExistentes);

        if (!result.Sucess)
        {
            return BadRequest(new RetornoErroModel { Erros = result.Notifications.ToList() });
        }

        return Ok(result);
    }
}
