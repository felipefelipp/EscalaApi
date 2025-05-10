using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IEscalaRepository
{
    Task<List<EscalaDto>> ObterEscalas();
    Task<EscalaDto> ObterEscalaPorId(int idEscala);
    Task InserirEscala(List<EscalaDto> escalaDto);
    Task<bool> AtualizarEscala(int idEscala, EscalaDto escalaDto);
}