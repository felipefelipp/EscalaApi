using EscalaApi.Data.Entities;
using EscalaApi.Services.Results;

namespace EscalaApi.Services.Interfaces;

public interface IEscalaManagerService
{
    Task<Result<List<Data.Entities.Escala>>> GerenciarEscala(EscalaIntegrantes escala);
}