using EscalaApi.Data.Entities;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class TipoGranularidadeService : ITipoGranularidadeService
{
    private readonly ITipoGranularidadeRepository _repository;

    public TipoGranularidadeService(ITipoGranularidadeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<TipoGranularidade>>> ListarAsync()
    {
        var dtos = await _repository.ListarAsync(apenasAtivos: false);
        return Result<List<TipoGranularidade>>.Ok(dtos.Select(Map).ToList());
    }

    public async Task<Result<TipoGranularidade>> ObterPorIdAsync(int id)
    {
        var dto = await _repository.ObterPorIdAsync(id);
        if (dto == null)
            return Result<TipoGranularidade>.NotFound([new Notification("Id", "Tipo de granularidade não encontrado.")]);
        return Result<TipoGranularidade>.Ok(Map(dto));
    }

    private static TipoGranularidade Map(Data.DTOs.TipoGranularidadeDto dto) => new()
    {
        Id = dto.IdTipoGranularidade,
        Codigo = dto.Codigo,
        Nome = dto.Nome,
        Descricao = dto.Descricao,
        Ativo = dto.Ativo
    };
}
