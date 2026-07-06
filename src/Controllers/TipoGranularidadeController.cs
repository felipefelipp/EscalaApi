using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
public class TipoGranularidadeController : ControllerBase
{
    private readonly ITipoGranularidadeService _service;

    public TipoGranularidadeController(ITipoGranularidadeService service)
    {
        _service = service;
    }

    [HttpGet("/tipos-granularidade")]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    [HttpGet("/tipos-granularidade/{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }
}
