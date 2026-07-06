using System.Net;
using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
public class TipoIntegranteController : ControllerBase
{
    private readonly ITipoIntegranteCatalogoService _service;

    public TipoIntegranteController(ITipoIntegranteCatalogoService service)
    {
        _service = service;
    }

    [HttpGet("/tipos-integrante")]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    [HttpGet("/tipos-integrante/existe")]
    public async Task<IActionResult> Existe()
    {
        var retorno = await _service.ExisteAsync();
        return Ok(new { existe = retorno.Object });
    }

    [HttpGet("/tipos-integrante/{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    [HttpPost("/tipos-integrante")]
    public async Task<IActionResult> Inserir(TipoIntegranteCatalogoRequest request)
    {
        var retorno = await _service.InserirAsync(request);
        if (!retorno.Sucess) return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return StatusCode((int)HttpStatusCode.Created, retorno.Object);
    }

    [HttpPut("/tipos-integrante/{id}")]
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

    [HttpDelete("/tipos-integrante/{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var retorno = await _service.ExcluirAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return NoContent();
    }
}
