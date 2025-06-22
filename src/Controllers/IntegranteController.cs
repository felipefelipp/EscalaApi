using System.Net;
using EscalaApi.Data.Request;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscalaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IntegranteController : ControllerBase
{
    private readonly IIntegranteService _integranteService;

    public IntegranteController(IIntegranteService integranteService)
    {
        _integranteService = integranteService;
    }

    [HttpGet("/integrantes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantes(int skip, int take)
    {
        var retorno = await _integranteService.ObterIntegrantes(skip, take);

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

    // <summary>
    // Obtém uma lista de integrantes com base no tipo fornecido.
    // </summary>
    // <param name="tipoIntegrante">
    // O tipo de integrante a ser filtrado. Valores possíveis:
    // - 0: Ministro
    // - 1: Backing Vocal
    // - 2: Instrumentista
    // </param>
    // <returns>
    // Retorna uma resposta HTTP com o seguinte comportamento:
    // - 200 OK: Se a operação for bem-sucedida, retorna a lista de integrantes.
    // - 404 NotFound: Se nenhum integrante for encontrado.
    // - 400 BadRequest: Se ocorrer algum erro de validação.
    // - 500 InternalServerError: Em caso de erro interno.
    // </returns>
    [HttpGet("/integrantes-tipo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterIntegrantesPorTipo([FromQuery] int tipoIntegrante)
    {
        var retorno = await _integranteService.ObterIntegrantesPorTipo(tipoIntegrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

    [HttpPost("/integrantes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RetornoErroModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InserirIntegrante(IntegranteRequest integrante)
    {
        var retorno = await _integranteService.InserirIntegrante(integrante);

        if (!retorno.Sucess)
        {
            if (retorno.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new RetornoErroModel { Erros = retorno.Notifications.ToList() });

            return BadRequest(new RetornoErroModel { Erros = retorno.Notifications.ToList() });
        }

        return Ok(retorno);
    }

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

    [HttpDelete("/integrantes/{idIntegrante}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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