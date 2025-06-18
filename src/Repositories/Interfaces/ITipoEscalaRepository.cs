namespace EscalaApi.Repositories.Interfaces;

public interface ITipoEscalaRepository
{
    Task<List<int>> ObterTiposEscalaDisponiveis();
}