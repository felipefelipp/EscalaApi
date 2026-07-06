using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
public class ParametroSistemaController : ControllerBase
{
    private readonly IParametroSistemaService _service;

    public ParametroSistemaController(IParametroSistemaService service)
    {
        _service = service;
    }

    [HttpGet("/parametros")]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    [HttpPut("/parametros/range-maximo")]
    public async Task<IActionResult> AtualizarRangeMaximo(RangeMaximoRequest request)
    {
        var retorno = await _service.AtualizarRangeMaximoAsync(request);
        if (!retorno.Sucess) return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }
}
