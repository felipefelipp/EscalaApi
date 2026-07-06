using System.Net;
using EscalaApi.Data.Entities;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using EscalaApi.Services.Rotacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EscalaController : ControllerBase
{
    private readonly IEscalaManagerService _escalaManagerService;
    private readonly IEscalaGeracaoService _escalaGeracaoService;

    public EscalaController(IEscalaManagerService escalaManagerService, IEscalaGeracaoService escalaGeracaoService)
    {
        _escalaManagerService = escalaManagerService;
        _escalaGeracaoService = escalaGeracaoService;
    }

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

    /// <summary>Gera preview de escala com token para persistência posterior.</summary>
    [HttpPost("/escalas/gerar")]
    public async Task<IActionResult> GerarEscala(GerarEscalaRequest request)
    {
        var retorno = await _escalaGeracaoService.GerarPreviewAsync(request);
        if (!retorno.Sucess)
            return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    /// <summary>Persiste exatamente o lote exibido no preview.</summary>
    [HttpPost("/escalas/preview/{token}/persistir")]
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

    [HttpPost("/escalas/import-csv")]
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