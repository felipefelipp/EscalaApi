using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Parâmetros globais que afetam validações em toda a API.</summary>
[ApiController]
[Tags("Parâmetros do sistema")]
public class ParametroSistemaController : ControllerBase
{
    private readonly IParametroSistemaService _service;

    public ParametroSistemaController(IParametroSistemaService service)
    {
        _service = service;
    }

    /// <summary>Lista parâmetros configuráveis (ex.: limite máximo de range de datas).</summary>
    [HttpGet("/parametros")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    /// <summary>Define o limite máximo do período em configurações (default: mensal). Range maior retorna 422.</summary>
    [HttpPut("/parametros/range-maximo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtualizarRangeMaximo(RangeMaximoRequest request)
    {
        var retorno = await _service.AtualizarRangeMaximoAsync(request);
        if (!retorno.Sucess) return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }
}
