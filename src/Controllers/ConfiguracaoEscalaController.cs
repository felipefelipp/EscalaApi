using System.Net;
using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
public class ConfiguracaoEscalaController : ControllerBase
{
    private readonly IConfiguracaoEscalaService _service;

    public ConfiguracaoEscalaController(IConfiguracaoEscalaService service)
    {
        _service = service;
    }

    [HttpGet("/configuracoes-escala")]
    public async Task<IActionResult> Listar()
    {
        var retorno = await _service.ListarAsync();
        return Ok(retorno.Object);
    }

    [HttpGet("/configuracoes-escala/{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        var retorno = await _service.ObterPorIdAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    [HttpGet("/configuracoes-escala/{id}/datas-expandidas")]
    public async Task<IActionResult> DatasExpandidas(int id)
    {
        var retorno = await _service.ObterDatasExpandidasAsync(id);
        if (!retorno.Sucess) return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        return Ok(retorno.Object);
    }

    [HttpPost("/configuracoes-escala")]
    public async Task<IActionResult> Inserir(ConfiguracaoEscalaRequest request)
    {
        var retorno = await _service.InserirAsync(request);
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.UnprocessableEntity)
                return UnprocessableEntity(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return StatusCode((int)HttpStatusCode.Created, retorno.Object);
    }

    [HttpPut("/configuracoes-escala/{id}")]
    public async Task<IActionResult> Atualizar(int id, ConfiguracaoEscalaRequest request)
    {
        var retorno = await _service.AtualizarAsync(id, request);
        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.Conflict)
                return Conflict(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.UnprocessableEntity)
                return UnprocessableEntity(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return Ok(retorno.Object);
    }
}
