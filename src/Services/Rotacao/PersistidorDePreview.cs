using EscalaApi.Data.DTOs;
using EscalaApi.Mappers;
using EscalaApi.Repositories.Interfaces;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Valida o token de preview e persiste o lote armazenado nas escalas definitivas.
/// </summary>
public sealed class PersistidorDePreview
{
    private readonly IArmazenamentoPreview _armazenamentoPreview;
    private readonly IEscalaRepository _escalaRepository;
    private readonly IConfiguracaoEscalaRepository _configuracaoRepository;

    public PersistidorDePreview(
        IArmazenamentoPreview armazenamentoPreview,
        IEscalaRepository escalaRepository,
        IConfiguracaoEscalaRepository configuracaoRepository)
    {
        _armazenamentoPreview = armazenamentoPreview;
        _escalaRepository = escalaRepository;
        _configuracaoRepository = configuracaoRepository;
    }

    public async Task<PersistenciaPreviewResultado> PersistirAsync(PreviewPersistRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PreviewToken))
            return PersistenciaPreviewResultado.Falha("Token de preview é obrigatório.");

        var preview = await _armazenamentoPreview.ObterPorTokenAsync(request.PreviewToken);
        if (preview is null)
            return PersistenciaPreviewResultado.Falha("Preview não encontrado para o token informado.");

        if (preview.Persistido)
            return PersistenciaPreviewResultado.Falha("Este preview já foi persistido.");

        if (preview.ExpiraEm < DateTime.UtcNow)
            return PersistenciaPreviewResultado.Falha("O token de preview expirou.");

        if (preview.Lote.Escalas.Count == 0)
            return PersistenciaPreviewResultado.Falha("O preview não contém escalas para persistir.");

        var dtos = preview.Lote.Escalas.ToList().ParaListaEscalaDto();
        foreach (var dto in dtos)
            dto.IdConfiguracao = preview.ConfiguracaoId;

        await _escalaRepository.InserirEscala(dtos);
        await _armazenamentoPreview.MarcarComoPersistidoAsync(request.PreviewToken);
        await _configuracaoRepository.MarcarEstrategiaImutavelAsync(preview.ConfiguracaoId);

        return PersistenciaPreviewResultado.ComSucesso(preview.Lote.Escalas.Count);
    }
}

public sealed class PersistenciaPreviewResultado
{
    public bool Sucesso { get; init; }
    public string? Mensagem { get; init; }
    public int EscalasPersistidas { get; init; }

    public static PersistenciaPreviewResultado ComSucesso(int quantidade) => new()
    {
        Sucesso = true,
        EscalasPersistidas = quantidade
    };

    public static PersistenciaPreviewResultado Falha(string mensagem) => new()
    {
        Sucesso = false,
        Mensagem = mensagem
    };
}
