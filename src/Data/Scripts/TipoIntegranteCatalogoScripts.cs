namespace EscalaApi.Data.Scripts;

public static class TipoIntegranteCatalogoScripts
{
    public const string Listar = @"
        SELECT id_tipo_integrante AS IdTipoIntegrante, desc_nome AS Nome, descricao AS Descricao,
               fl_ativo AS Ativo, dt_criacao AS DataCriacao
        FROM tipo_integrante_catalogo WHERE fl_ativo = 1 ORDER BY desc_nome";

    public const string ListarTodos = @"
        SELECT id_tipo_integrante AS IdTipoIntegrante, desc_nome AS Nome, descricao AS Descricao,
               fl_ativo AS Ativo, dt_criacao AS DataCriacao
        FROM tipo_integrante_catalogo ORDER BY desc_nome";

    public const string ObterPorId = @"
        SELECT id_tipo_integrante AS IdTipoIntegrante, desc_nome AS Nome, descricao AS Descricao,
               fl_ativo AS Ativo, dt_criacao AS DataCriacao
        FROM tipo_integrante_catalogo WHERE id_tipo_integrante = @Id";

    public const string ContarAtivos = @"SELECT COUNT(*) FROM tipo_integrante_catalogo WHERE fl_ativo = 1";

    public const string Inserir = @"
        INSERT INTO tipo_integrante_catalogo (desc_nome, descricao) VALUES (@Nome, @Descricao);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

    public const string Atualizar = @"
        UPDATE tipo_integrante_catalogo SET desc_nome = @Nome, descricao = @Descricao
        WHERE id_tipo_integrante = @Id";

    public const string ExcluirSoft = @"
        UPDATE tipo_integrante_catalogo SET fl_ativo = 0 WHERE id_tipo_integrante = @Id";

    public const string ExisteNome = @"
        SELECT COUNT(*) FROM tipo_integrante_catalogo WHERE desc_nome = @Nome AND id_tipo_integrante <> @IdExcluir";
}
