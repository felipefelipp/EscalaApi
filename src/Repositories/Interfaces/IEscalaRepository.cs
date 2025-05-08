using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IEscalaRepository
{
    Task<List<Data.Entities.Escala>> ObterEscalas();
    Task InserirEscala(List<EscalaDto> escala);
}