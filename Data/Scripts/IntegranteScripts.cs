namespace EscalaApi.Data.Scripts;

public static class IntegranteScripts
{
    public const string ObterIntegrantePorIdScript = @"
             SELECT 
                 Integrantes.nome AS Nome, 
                 Integrantes_dias_disponiveis.dia_disponivel AS DiaDisponivel, 
                 Tipo_integrante.tipo_integrante AS TipoIntegrante
             FROM Integrantes
             LEFT JOIN Integrantes_dias_disponiveis
                 ON Integrantes.id_integrante = Integrantes_dias_disponiveis.id_integrante
             LEFT JOIN Tipo_integrante
                 ON Integrantes.id_integrante = Tipo_integrante.id_integrante 
             WHERE Integrantes.id_integrante = @IdIntegrante";

    public const string ObterIntegrantePorTipoScript = @"
             SELECT 
                 Integrantes.id_integrante AS IdIntegrante,
                 Integrantes.nome AS Nome, 
                 Integrantes_dias_disponiveis.dia_disponivel AS DiaDisponivel, 
                 Tipo_integrante.tipo_integrante AS TipoIntegrante
             FROM Integrantes
             INNER JOIN Integrantes_dias_disponiveis
                 ON Integrantes.id_integrante = Integrantes_dias_disponiveis.id_integrante
             INNER JOIN Tipo_integrante
                 ON Integrantes.id_integrante = Tipo_integrante.id_integrante 
             WHERE Tipo_integrante.tipo_integrante = @TipoIntegrante";

    public const string ObterTodosOsIntegrantes = @"
            SELECT 
                 Integrantes.id_integrante AS IdIntegrante,
                 Integrantes.nome AS Nome, 
                 Integrantes_dias_disponiveis.dia_disponivel AS DiaDisponivel, 
                 Tipo_integrante.tipo_integrante AS TipoIntegrante
             FROM Integrantes
             LEFT JOIN Integrantes_dias_disponiveis
                 ON Integrantes.id_integrante = Integrantes_dias_disponiveis.id_integrante
             LEFT JOIN Tipo_integrante
                 ON Integrantes.id_integrante = Tipo_integrante.id_integrante 
             ORDER BY Integrantes.id_integrante
             LIMIT @PageSize OFFSET @Offset";

    public const string QuantidadeIntegrantes = @" 
                 SELECT COUNT(id_integrante) Total
                    FROM (
                    SELECT
                       DISTINCT Integrantes.id_integrante id_integrante
                    FROM Integrantes
                             INNER JOIN Integrantes_dias_disponiveis
                                        ON Integrantes.id_integrante = Integrantes_dias_disponiveis.id_integrante
                             INNER JOIN Tipo_integrante
                                        ON Integrantes.id_integrante = Tipo_integrante.id_integrante) AS Integrantes";

    public const string InserirIntegrante = @"INSERT INTO Integrantes(nome) VALUES(@nome);
                                              SELECT last_insert_rowid();";
    
    public const string AtualizarIntegrante = @"UPDATE Integrantes
                                                SET nome = @nome
                                                WHERE id_integrante = @idIntegrante";
    
    public const string RemoverIntegrante = @"DELETE FROM Integrantes WHERE id_integrante = @idIntegrante";
}