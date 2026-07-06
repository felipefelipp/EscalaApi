using System.Text.Json;
using EscalaApi.Data.Entities;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Armazenamento temporário de lotes de preview indexados por token.
/// </summary>
public interface IArmazenamentoPreview
{
    Task<string> SalvarAsync(LoteDeEscalas lote, int configuracaoId, DateTime expiraEm, string codigoEstrategia);
    Task<PreviewArmazenado?> ObterPorTokenAsync(string token);
    Task MarcarComoPersistidoAsync(string token);
}

public sealed class PreviewArmazenado
{
    public string Token { get; init; } = string.Empty;
    public int ConfiguracaoId { get; init; }
    public LoteDeEscalas Lote { get; init; } = new();
    public DateTime ExpiraEm { get; init; }
    public DateTime CriadoEm { get; init; }
    public bool Persistido { get; set; }
    public string CodigoEstrategia { get; init; } = string.Empty;
}

/// <summary>
/// Implementação em memória para testes e desenvolvimento até existir repositório SQL.
/// </summary>
public sealed class ArmazenamentoPreviewMemoria : IArmazenamentoPreview
{
    private readonly Dictionary<string, PreviewArmazenado> _previews = new();

    public Task<string> SalvarAsync(LoteDeEscalas lote, int configuracaoId, DateTime expiraEm, string codigoEstrategia)
    {
        var token = Guid.NewGuid().ToString("N");
        _previews[token] = new PreviewArmazenado
        {
            Token = token,
            ConfiguracaoId = configuracaoId,
            Lote = lote,
            ExpiraEm = expiraEm,
            CriadoEm = DateTime.UtcNow,
            CodigoEstrategia = codigoEstrategia
        };

        return Task.FromResult(token);
    }

    public Task<PreviewArmazenado?> ObterPorTokenAsync(string token)
    {
        _previews.TryGetValue(token, out var preview);
        return Task.FromResult(preview);
    }

    public Task MarcarComoPersistidoAsync(string token)
    {
        if (_previews.TryGetValue(token, out var preview))
            preview.Persistido = true;

        return Task.CompletedTask;
    }
}
