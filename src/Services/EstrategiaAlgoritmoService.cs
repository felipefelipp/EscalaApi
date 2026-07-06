using EscalaApi.Data.Entities;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class EstrategiaAlgoritmoService : IEstrategiaAlgoritmoService
{
    private readonly IEstrategiaAlgoritmoRepository _repository;

    public EstrategiaAlgoritmoService(IEstrategiaAlgoritmoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<EstrategiaAlgoritmo>>> ListarAsync()
    {
        var dtos = await _repository.ListarAsync();
        return Result<List<EstrategiaAlgoritmo>>.Ok(dtos.Select(Map).ToList());
    }

    public async Task<Result<EstrategiaAlgoritmo>> ObterPorIdAsync(int id)
    {
        var dto = await _repository.ObterPorIdAsync(id);
        if (dto == null)
            return Result<EstrategiaAlgoritmo>.NotFound([new Notification("Id", "Estratégia de algoritmo não encontrada.")]);
        return Result<EstrategiaAlgoritmo>.Ok(Map(dto));
    }

    private static EstrategiaAlgoritmo Map(Data.DTOs.EstrategiaAlgoritmoDto dto) => new()
    {
        Id = dto.IdEstrategiaAlgoritmo,
        Codigo = dto.Codigo,
        Nome = dto.Nome,
        DescricaoDetalhada = dto.DescricaoDetalhada,
        Ativo = dto.Ativo
    };
}
