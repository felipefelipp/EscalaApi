using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface ITipoGranularidadeRepository
{
    Task<List<TipoGranularidadeDto>> ListarAsync(bool apenasAtivos = true);
    Task<TipoGranularidadeDto?> ObterPorIdAsync(int id);
}
