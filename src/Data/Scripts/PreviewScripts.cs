namespace EscalaApi.Data.Scripts;

public static class PreviewScripts
{
    public const string Inserir = @"
        INSERT INTO escala_preview (token, id_configuracao, payload_json, dt_expiracao)
        VALUES (@Token, @IdConfiguracao, @PayloadJson, @Expiracao);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

    public const string InvalidarAnteriores = @"
        UPDATE escala_preview SET fl_persistido = 1
        WHERE id_configuracao = @IdConfiguracao AND fl_persistido = 0";

    public const string ObterPorToken = @"
        SELECT id_preview AS IdPreview, token AS Token, id_configuracao AS ConfiguracaoId,
               payload_json AS PayloadJson, dt_criacao AS CriadoEm, dt_expiracao AS ExpiraEm,
               fl_persistido AS Persistido
        FROM escala_preview WHERE token = @Token";

    public const string MarcarPersistido = @"
        UPDATE escala_preview SET fl_persistido = 1 WHERE token = @Token";

    public const string LimparExpirados = @"
        DELETE FROM escala_preview WHERE fl_persistido = 0 AND dt_expiracao < GETUTCDATE()";
}
