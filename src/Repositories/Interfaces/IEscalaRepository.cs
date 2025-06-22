using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;

namespace EscalaApi.Repositories.Interfaces;

public interface IEscalaRepository
{
    Task<List<EscalaDto>> ObterEscalas(EscalaFiltro escalaFiltro);
    Task<EscalaDto> ObterEscalaPorId(int idEscala);
    Task InserirEscala(List<EscalaDto> escalaDto);
    Task<bool> AtualizarEscala(int idEscala, EscalaDto escalaDto);
}