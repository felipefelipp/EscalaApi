namespace EscalaApi.Data.Scripts;

public static class ParametroSistemaScripts
{
    public const string Listar = @"
        SELECT id_parametro AS IdParametro, chave AS Chave, valor AS Valor,
               descricao AS Descricao, dt_atualizacao AS DataAtualizacao
        FROM parametro_sistema ORDER BY chave";

    public const string ObterPorChave = @"
        SELECT id_parametro AS IdParametro, chave AS Chave, valor AS Valor,
               descricao AS Descricao, dt_atualizacao AS DataAtualizacao
        FROM parametro_sistema WHERE chave = @Chave";

    public const string AtualizarPorChave = @"
        UPDATE parametro_sistema SET valor = @Valor, dt_atualizacao = GETUTCDATE()
        WHERE chave = @Chave";

    public const string Inserir = @"
        INSERT INTO parametro_sistema (chave, valor, descricao)
        VALUES (@Chave, @Valor, @Descricao);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";
}
