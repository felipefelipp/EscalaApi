using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
public class EstrategiaAlgoritmoController : ControllerBase
{
    private readonly IEstrategiaAlgoritmoService _service;

    public EstrategiaAlgoritmoController(IEstrategiaAlgoritmoService service)
    {
        _service = service;
    }

    [HttpGet("/estrategias-algoritmo")]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    [HttpGet("/estrategias-algoritmo/{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }
}
