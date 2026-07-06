using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Catálogo de granularidade temporal (ex.: dias da semana). Define como slots são expandidos em datas.</summary>
[ApiController]
[Tags("Catálogos do sistema")]
public class TipoGranularidadeController : ControllerBase
{
    private readonly ITipoGranularidadeService _service;

    public TipoGranularidadeController(ITipoGranularidadeService service)
    {
        _service = service;
    }

    /// <summary>Lista tipos de granularidade cadastrados. Somente leitura.</summary>
    [HttpGet("/tipos-granularidade")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    /// <summary>Obtém um tipo de granularidade pelo ID.</summary>
    [HttpGet("/tipos-granularidade/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }
}
