namespace EscalaApi.Data.Scripts;

public static class IntegranteScripts
{
    public const string ObterIntegrantePorIdScript = @"
             SELECT DISTINCT
                integrantes.id_integrante AS IdIntegrante,
                integrantes.desc_nome AS Nome, 
                integrantes_dias_disponiveis.cd_dia_disponivel AS DiaDaSemanaDisponivel, 
                integrante_tipo.cd_tipo_integrante AS TipoIntegrante
            FROM integrantes
            LEFT JOIN integrantes_dias_disponiveis
                ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
            LEFT JOIN integrante_tipo
                ON integrantes.id_integrante = integrante_tipo.id_integrante 
             WHERE integrantes.id_integrante = @IdIntegrante";

    public const string ObterIntegrantesPorTipoScript = @"
                SELECT DISTINCT
                    integrantes.id_integrante AS IdIntegrante,
                    integrantes.desc_nome AS Nome, 
                    integrantes_dias_disponiveis.cd_dia_disponivel AS DiaDaSemanaDisponivel, 
                    integrante_tipo.cd_tipo_integrante AS TipoIntegrante
                FROM integrantes
                LEFT JOIN integrantes_dias_disponiveis
                    ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
                LEFT JOIN integrante_tipo
                    ON integrantes.id_integrante = integrante_tipo.id_integrante 
                WHERE integrante_tipo.cd_tipo_integrante = @TipoIntegrante";

    /// <summary>
    /// FROM base para filtrar IDs de integrantes (joins necessários aos filtros de dia/tipo).
    /// </summary>
    public const string FromIntegrantesComJoins = @"
            FROM integrantes
            LEFT JOIN integrantes_dias_disponiveis
                ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
            LEFT JOIN integrante_tipo
                ON integrantes.id_integrante = integrante_tipo.id_integrante";

    /// <summary>
    /// Dados completos (dias + tipos) a partir de uma CTE de IDs já paginados/filtrados.
    /// </summary>
    public const string SelecionarIntegrantesCompletosPorIds = @"
            SELECT DISTINCT
                integrantes.id_integrante AS IdIntegrante,
                integrantes.desc_nome AS Nome,
                integrantes_dias_disponiveis.cd_dia_disponivel AS DiaDaSemanaDisponivel,
                integrante_tipo.cd_tipo_integrante AS TipoIntegrante
            FROM {0}
            INNER JOIN integrantes
                ON integrantes.id_integrante = {0}.id_integrante
            LEFT JOIN integrantes_dias_disponiveis
                ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
            LEFT JOIN integrante_tipo
                ON integrantes.id_integrante = integrante_tipo.id_integrante
            ORDER BY integrantes.id_integrante";

    public const string InserirIntegrante = @"INSERT INTO integrantes(desc_nome) VALUES(@nome);
                                              SELECT SCOPE_IDENTITY();";
    
    public const string AtualizarIntegrante = @"UPDATE integrantes
                                                SET desc_nome = @nome
                                                WHERE id_integrante = @idIntegrante";

    public const string RemoverIntegrante = @"DELETE FROM integrantes WHERE id_integrante = @idIntegrante";
}
