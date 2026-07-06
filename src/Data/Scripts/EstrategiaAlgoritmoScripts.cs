namespace EscalaApi.Data.Scripts;

public static class EstrategiaAlgoritmoScripts
{
    public const string Listar = @"
        SELECT id_estrategia_algoritmo AS IdEstrategiaAlgoritmo, codigo AS Codigo,
               txt_nome AS Nome, txt_descricao_detalhada AS DescricaoDetalhada, fl_ativo AS Ativo
        FROM estrategia_algoritmo WHERE fl_ativo = 1 ORDER BY id_estrategia_algoritmo";

    public const string ListarTodos = @"
        SELECT id_estrategia_algoritmo AS IdEstrategiaAlgoritmo, codigo AS Codigo,
               txt_nome AS Nome, txt_descricao_detalhada AS DescricaoDetalhada, fl_ativo AS Ativo
        FROM estrategia_algoritmo ORDER BY id_estrategia_algoritmo";

    public const string ObterPorId = @"
        SELECT id_estrategia_algoritmo AS IdEstrategiaAlgoritmo, codigo AS Codigo,
               txt_nome AS Nome, txt_descricao_detalhada AS DescricaoDetalhada, fl_ativo AS Ativo
        FROM estrategia_algoritmo WHERE id_estrategia_algoritmo = @Id";
}
