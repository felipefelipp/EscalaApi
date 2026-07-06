namespace EscalaApi.Data.Scripts;

public static class TipoGranularidadeScripts
{
    public const string Listar = @"
        SELECT id_tipo_granularidade AS IdTipoGranularidade, codigo AS Codigo,
               txt_nome AS Nome, txt_descricao AS Descricao, fl_ativo AS Ativo
        FROM tipo_granularidade WHERE fl_ativo = 1 ORDER BY id_tipo_granularidade";

    public const string ListarTodos = @"
        SELECT id_tipo_granularidade AS IdTipoGranularidade, codigo AS Codigo,
               txt_nome AS Nome, txt_descricao AS Descricao, fl_ativo AS Ativo
        FROM tipo_granularidade ORDER BY id_tipo_granularidade";

    public const string ObterPorId = @"
        SELECT id_tipo_granularidade AS IdTipoGranularidade, codigo AS Codigo,
               txt_nome AS Nome, txt_descricao AS Descricao, fl_ativo AS Ativo
        FROM tipo_granularidade WHERE id_tipo_granularidade = @Id";
}
