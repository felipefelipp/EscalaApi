using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IEscalaManagerService
{
    Task<Result<List<Escala>>> CriarEscala(EscalaIntegrantes escala);
    Task<Result<List<Escala>>> ObterEscalas();
}