namespace EscalaApi.Data.Scripts;

public static class IntegranteScripts
{
    public const string ObterIntegrantePorIdScript = @"
             SELECT DISTINCT
                integrantes.id_integrante AS IdIntegrante,
                integrantes.desc_nome AS Nome, 
                integrantes_dias_disponiveis.cd_dia_disponivel AS DiaDaSemanaDisponivel, 
                tipo_integrante.cd_tipo_integrante AS TipoIntegrante
            FROM integrantes
            LEFT JOIN integrantes_dias_disponiveis
                ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
            LEFT JOIN tipo_integrante
                ON integrantes.id_integrante = tipo_integrante.id_integrante 
             WHERE integrantes.id_integrante = @IdIntegrante";

    public const string ObterIntegrantesPorTipoScript = @"
                SELECT DISTINCT
                    integrantes.id_integrante AS IdIntegrante,
                    integrantes.desc_nome AS Nome, 
                    integrantes_dias_disponiveis.cd_dia_disponivel AS DiaDaSemanaDisponivel, 
                    tipo_integrante.cd_tipo_integrante AS TipoIntegrante
                FROM integrantes
                LEFT JOIN integrantes_dias_disponiveis
                    ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
                LEFT JOIN tipo_integrante
                    ON integrantes.id_integrante = tipo_integrante.id_integrante 
                WHERE tipo_integrante.cd_tipo_integrante = @TipoIntegrante";

    public const string ObterTodosOsintegrantes = @"
            SELECT DISTINCT
                integrantes.id_integrante AS IdIntegrante,
                integrantes.desc_nome AS Nome, 
                integrantes_dias_disponiveis.cd_dia_disponivel AS DiaDaSemanaDisponivel, 
                tipo_integrante.cd_tipo_integrante AS TipoIntegrante
            FROM integrantes
            LEFT JOIN integrantes_dias_disponiveis
                ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
            LEFT JOIN tipo_integrante
                ON integrantes.id_integrante = tipo_integrante.id_integrante";

    public const string Quantidadeintegrantes = @" 
                 SELECT COUNT(id_integrante) Total
                    FROM (
                    SELECT
                       DISTINCT integrantes.id_integrante id_integrante
                    FROM integrantes
                             INNER JOIN integrantes_dias_disponiveis
                                        ON integrantes.id_integrante = integrantes_dias_disponiveis.id_integrante
                             INNER JOIN tipo_integrante
                                        ON integrantes.id_integrante = tipo_integrante.id_integrante) AS integrantes";

    public const string InserirIntegrante = @"INSERT INTO integrantes(desc_nome) VALUES(@nome);
                                              SELECT SCOPE_IDENTITY();";
    
    public const string AtualizarIntegrante = @"UPDATE integrantes
                                                SET desc_nome = @nome
                                                WHERE id_integrante = @idIntegrante";

    public const string RemoverIntegrante = @"DELETE FROM integrantes WHERE id_integrante = @idIntegrante";
}