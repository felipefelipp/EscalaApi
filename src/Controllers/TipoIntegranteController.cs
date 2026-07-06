using System.Net;
using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Catálogo de papéis/funções na escala. Pré-requisito para cadastrar integrantes.</summary>
[ApiController]
[Tags("Tipos de integrante")]
public class TipoIntegranteController : ControllerBase
{
    private readonly ITipoIntegranteCatalogoService _service;

    public TipoIntegranteController(ITipoIntegranteCatalogoService service)
    {
        _service = service;
    }

    /// <summary>Lista todos os tipos do catálogo.</summary>
    [HttpGet("/tipos-integrante")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    /// <summary>Indica se o catálogo já foi inicializado (gatekeeping de integrantes).</summary>
    [HttpGet("/tipos-integrante/existe")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Existe()
    {
        var retorno = await _service.ExisteAsync();
        return Ok(new { existe = retorno.Object });
    }

    /// <summary>Obtém um tipo pelo ID.</summary>
    [HttpGet("/tipos-integrante/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    /// <summary>Cadastra um novo tipo de integrante.</summary>
    [HttpPost("/tipos-integrante")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Inserir(TipoIntegranteCatalogoRequest request)
    {
        var retorno = await _service.InserirAsync(request);
        if (!retorno.Sucess) return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return StatusCode((int)HttpStatusCode.Created, retorno.Object);
    }

    /// <summary>Atualiza nome ou metadados de um tipo existente.</summary>
    [HttpPut("/tipos-integrante/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, TipoIntegranteCatalogoRequest request)
    {
        var retorno = await _service.AtualizarAsync(id, request);
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return Ok(retorno.Object);
    }

    /// <summary>Remove um tipo do catálogo.</summary>
    [HttpDelete("/tipos-integrante/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        var retorno = await _service.ExcluirAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return NoContent();
    }
}
