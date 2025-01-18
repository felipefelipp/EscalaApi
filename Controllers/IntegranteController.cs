using System.Net;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using EscalaApi.Utils.Enums;
using EscalaApi.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IntegranteController : ControllerBase
{
    private readonly IIntegranteService _integranteService;

    public IntegranteController(IIntegranteService integranteService)
    {
        _integranteService = integranteService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantePorId(int idIntegrante)
    {
        var retorno = await _integranteService.ObterIntegrantePorId(idIntegrante);
        
        if (!retorno.Sucess)
        {
            
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        
        return Ok(retorno);
    }
    
    [HttpGet("/Integrantes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantePorId(TipoIntegrante tipoIntegrante)
    {
        var retorno = await _integranteService.ObterIntegrantesPorTipo(tipoIntegrante);
        
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        
        return Ok(retorno);
    }
}