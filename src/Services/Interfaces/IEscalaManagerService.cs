using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IEscalaManagerService
{
    Task<Result<Escala>> ObterEscalaPorId(int idEscala);
    Task<Result<List<Escala>>> CriarEscala(EscalaIntegrantes escala);
    Task<Result<List<Escala>>> ObterEscalas();
    Task<Result<EscalaIntegrante>> EditarEscala(int id, EscalaIntegrante escala);
}