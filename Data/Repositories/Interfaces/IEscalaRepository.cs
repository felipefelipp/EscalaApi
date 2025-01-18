namespace EscalaApi.Data.Repositories.Interfaces;

public interface IEscalaRepository
{
    Task<List<Entities.Escala>> ObterEscalas();
    Task InserirEscala(List<Entities.Escala> escala);
}