using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Catálogo de critérios de balanceamento na geração. Vinculado à configuração de escala.</summary>
[ApiController]
[Tags("Catálogos do sistema")]
public class EstrategiaAlgoritmoController : ControllerBase
{
    private readonly IEstrategiaAlgoritmoService _service;

    public EstrategiaAlgoritmoController(IEstrategiaAlgoritmoService service)
    {
        _service = service;
    }

    /// <summary>Lista estratégias disponíveis (contextual por dia da semana, contagem global). Somente leitura.</summary>
    [HttpGet("/estrategias-algoritmo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    /// <summary>Obtém detalhes de uma estratégia pelo ID.</summary>
    [HttpGet("/estrategias-algoritmo/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }
}
