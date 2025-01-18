using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IEscalaManagerService
{
    Task<Result> CriarEscala(EscalaIntegrantes escala);
    Task<Result<List<Data.Entities.Escala>>> ObterEscalas();
}