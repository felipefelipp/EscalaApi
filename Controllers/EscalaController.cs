using System.Net;
using EscalaApi.Data.Entities;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EscalaController : ControllerBase
{
    private readonly IEscalaManagerService _escalaManagerService;

    public EscalaController(IEscalaManagerService escalaManagerService)
    {
        _escalaManagerService = escalaManagerService;
    }

    [HttpPost]
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

        return Created();
    }
    
    [HttpGet("/Escalas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterEscalas()
    {
        var retorno = await _escalaManagerService.ObterEscalas();
        
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        
        return Ok(retorno);
    }
}