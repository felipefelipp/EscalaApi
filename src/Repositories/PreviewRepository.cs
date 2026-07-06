using System.Text.Json;
using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.Entities;
using EscalaApi.Data.Scripts;
using EscalaApi.Services.Rotacao;

namespace EscalaApi.Repositories;

public class PreviewRepository : IArmazenamentoPreview
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<string> SalvarAsync(LoteDeEscalas lote, int configuracaoId, DateTime expiraEm, string codigoEstrategia)
    {
        using var connection = DatabaseContext.GetConnection();
        await connection.ExecuteAsync(PreviewScripts.InvalidarAnteriores, new { IdConfiguracao = configuracaoId });

        var token = Guid.NewGuid().ToString("N");
        var payload = JsonSerializer.Serialize(lote, JsonOptions);

        await connection.ExecuteScalarAsync<int>(PreviewScripts.Inserir, new
        {
            Token = token,
            IdConfiguracao = configuracaoId,
            PayloadJson = payload,
            Expiracao = expiraEm
        });

        return token;
    }

    public async Task<PreviewArmazenado?> ObterPorTokenAsync(string token)
    {
        using var connection = DatabaseContext.GetConnection();
        var row = await connection.QueryFirstOrDefaultAsync<PreviewRow>(PreviewScripts.ObterPorToken, new { Token = token });
        if (row is null) return null;

        var lote = JsonSerializer.Deserialize<LoteDeEscalas>(row.PayloadJson, JsonOptions) ?? new LoteDeEscalas();
        return new PreviewArmazenado
        {
            Token = row.Token,
            ConfiguracaoId = row.ConfiguracaoId,
            Lote = lote,
            ExpiraEm = row.ExpiraEm,
            CriadoEm = row.CriadoEm,
            Persistido = row.Persistido,
            CodigoEstrategia = string.Empty
        };
    }

    public async Task MarcarComoPersistidoAsync(string token) =>
        await DatabaseContext.GetConnection().ExecuteAsync(PreviewScripts.MarcarPersistido, new { Token = token });

    public async Task LimparExpiradosAsync() =>
        await DatabaseContext.GetConnection().ExecuteAsync(PreviewScripts.LimparExpirados);

    private sealed class PreviewRow
    {
        public string Token { get; set; } = string.Empty;
        public int ConfiguracaoId { get; set; }
        public string PayloadJson { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
        public DateTime ExpiraEm { get; set; }
        public bool Persistido { get; set; }
    }
}
