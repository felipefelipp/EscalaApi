using EscalaApi.Data.Entities;
using EscalaApi.Data.Request;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Interfaces;
using EscalaApi.Services.Results;
using Flunt.Notifications;

namespace EscalaApi.Services;

public class TipoIntegranteCatalogoService : ITipoIntegranteCatalogoService
{
    private readonly ITipoIntegranteCatalogoRepository _repository;

    public TipoIntegranteCatalogoService(ITipoIntegranteCatalogoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<TipoIntegranteCatalogo>>> ListarAsync()
    {
        var dtos = await _repository.ListarAsync();
        return Result<List<TipoIntegranteCatalogo>>.Ok(dtos.Select(Map).ToList());
    }

    public async Task<Result<TipoIntegranteCatalogo>> ObterPorIdAsync(int id)
    {
        var dto = await _repository.ObterPorIdAsync(id);
        if (dto == null)
            return Result<TipoIntegranteCatalogo>.NotFound([new Notification("Id", "Tipo de integrante não encontrado.")]);
        return Result<TipoIntegranteCatalogo>.Ok(Map(dto));
    }

    public async Task<Result<bool>> ExisteAsync()
    {
        var count = await _repository.ContarAtivosAsync();
        return Result<bool>.Ok(count > 0);
    }

    public async Task<Result<TipoIntegranteCatalogo>> InserirAsync(TipoIntegranteCatalogoRequest request)
    {
        var erros = Validar(request);
        if (erros.Count > 0) return Result<TipoIntegranteCatalogo>.BadRequest(erros);
        if (await _repository.ExisteNomeAsync(request.Nome))
            return Result<TipoIntegranteCatalogo>.BadRequest([new Notification("Nome", "Já existe um tipo com este nome.")]);

        var id = await _repository.InserirAsync(request);
        var criado = await _repository.ObterPorIdAsync(id);
        return Result<TipoIntegranteCatalogo>.Created(Map(criado!));
    }

    public async Task<Result<TipoIntegranteCatalogo>> AtualizarAsync(int id, TipoIntegranteCatalogoRequest request)
    {
        var erros = Validar(request);
        if (erros.Count > 0) return Result<TipoIntegranteCatalogo>.BadRequest(erros);

        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null)
            return Result<TipoIntegranteCatalogo>.NotFound([new Notification("Id", "Tipo de integrante não encontrado.")]);

        if (await _repository.ExisteNomeAsync(request.Nome, id))
            return Result<TipoIntegranteCatalogo>.BadRequest([new Notification("Nome", "Já existe um tipo com este nome.")]);

        await _repository.AtualizarAsync(id, request);
        var atualizado = await _repository.ObterPorIdAsync(id);
        return Result<TipoIntegranteCatalogo>.Ok(Map(atualizado!));
    }

    public async Task<Result<bool>> ExcluirAsync(int id)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null)
            return Result<bool>.NotFound([new Notification("Id", "Tipo de integrante não encontrado.")]);

        await _repository.ExcluirSoftAsync(id);
        return Result<bool>.Ok(true);
    }

    private static List<Notification> Validar(TipoIntegranteCatalogoRequest request)
    {
        var erros = new List<Notification>();
        if (string.IsNullOrWhiteSpace(request.Nome) || request.Nome.Length < 2)
            erros.Add(new Notification("Nome", "Nome é obrigatório (mínimo 2 caracteres)."));
        if (request.Nome?.Length > 100)
            erros.Add(new Notification("Nome", "Nome não pode exceder 100 caracteres."));
        return erros;
    }

    private static TipoIntegranteCatalogo Map(Data.DTOs.TipoIntegranteCatalogoDto dto) => new()
    {
        IdTipoIntegrante = dto.IdTipoIntegrante,
        Nome = dto.Nome,
        Descricao = dto.Descricao,
        Ativo = dto.Ativo,
        DataCriacao = dto.DataCriacao
    };
}
