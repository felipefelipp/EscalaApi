using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IEscalaManagerService
{
    Task<Result<EscalaResponse>> ObterEscalaPorId(int idEscala);
    Task<Result<List<Escala>>> CriarEscala(EscalaIntegrantes escala);
    Task<Result<List<EscalaResponse>>> ObterEscalas(EscalaFiltro escalaFiltro);
    Task<Result<EscalaIntegrante>> EditarEscala(int id, EscalaIntegrante escala);
    Task<Result<EscalaResponse>> ImportarEscalasDeCsv(IFormFile csvContent, bool substituirExistentes);
}