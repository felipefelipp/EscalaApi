namespace EscalaApi.Data.Scripts;

public static class IntegranteScripts
{
    public const string ObterIntegrantePorIdScript = @"
             SELECT 
                 Integrantes.nome AS Nome, 
                 Integrantes_dias_disponiveis.dia_disponivel AS DiaDisponivel, 
                 Tipo_integrante.tipo_integrante AS TipoIntegrante
             FROM Integrantes
             INNER JOIN Integrantes_dias_disponiveis
                 ON Integrantes.id_integrante = Integrantes_dias_disponiveis.id_integrante
             INNER JOIN Tipo_integrante
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
                 Integrantes.nome AS Nome, 
                 Integrantes_dias_disponiveis.dia_disponivel AS DiaDisponivel, 
                 Tipo_integrante.tipo_integrante AS TipoIntegrante
             FROM Integrantes
             INNER JOIN Integrantes_dias_disponiveis
                 ON Integrantes.id_integrante = Integrantes_dias_disponiveis.id_integrante
             INNER JOIN Tipo_integrante
                 ON Integrantes.id_integrante = Tipo_integrante.id_integrante;";
}