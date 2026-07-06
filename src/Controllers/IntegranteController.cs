using System.Net;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

/// <summary>Pessoas elegíveis para escala, com tipos e disponibilidade. Alimentam o algoritmo de rotação.</summary>
[ApiController]
[Route("[controller]")]
[Tags("Integrantes")]
public class IntegranteController : ControllerBase
{
    private readonly IIntegranteService _integranteService;

    public IntegranteController(IIntegranteService integranteService)
    {
        _integranteService = integranteService;
    }

    /// <summary>Lista integrantes com paginação e filtros opcionais via query string.</summary>
    [HttpGet("/integrantes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantes([FromQuery] IntegranteFiltro filtro)
    {
        var retorno = await _integranteService.ObterIntegrantes(filtro);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(
            new
            {
                sucess = retorno.Sucess,
                retorno.Object.integrantes,
                retorno.Object.total,
                statusCode = retorno.StatusCode,
                isValid = retorno.IsValid
            }
            );
    }

    /// <summary>Obtém um integrante pelo ID, com tipos e disponibilidade.</summary>
    [HttpGet("/integrantes/{idIntegrante}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantes(int idIntegrante)
    {
        var retorno = await _integranteService.ObterIntegrantePorId(idIntegrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Lista integrantes habilitados para um tipo do catálogo (pool de candidatos por função).</summary>
    /// <param name="tipoIntegrante">ID do tipo em <c>/tipos-integrante</c>.</param>
    [HttpGet("/integrantes-tipo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantesPorTipo([FromQuery] int tipoIntegrante)
    {
        var retorno = await _integranteService.ObterIntegrantesPorTipo(tipoIntegrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.UnprocessableEntity)
                return UnprocessableEntity(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Cadastra integrante. Exige ao menos um tipo; retorna 422 se o catálogo estiver vazio.</summary>
    [HttpPost("/integrantes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InserirIntegrante(IntegranteRequest integrante)
    {
        var retorno = await _integranteService.InserirIntegrante(integrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
            if (retorno.StatusCode == HttpStatusCode.UnprocessableEntity)
                return UnprocessableEntity(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    /// <summary>Atualiza dados, tipos e disponibilidade de um integrante existente.</summary>
    [HttpPut("/integrantes/{idIntegrante}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarIntegrante(int idIntegrante, IntegranteRequest integrante)
    {
        var retorno = await _integranteService.AtualizarIntegrante(idIntegrante, integrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return Ok(retorno);
    }

    /// <summary>Remove um integrante do sistema.</summary>
    [HttpDelete("/integrantes/{idIntegrante}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExcluirIntegrante(int idIntegrante)
    {
        var retorno = await _integranteService.ExcluirIntegrante(idIntegrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }
        return NoContent();
    }
}
