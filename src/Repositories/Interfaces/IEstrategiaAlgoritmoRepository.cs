using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IEstrategiaAlgoritmoRepository
{
    Task<List<EstrategiaAlgoritmoDto>> ListarAsync(bool apenasAtivos = true);
    Task<EstrategiaAlgoritmoDto?> ObterPorIdAsync(int id);
}
