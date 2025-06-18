namespace EscalaApi.Data.Scripts;

public class EscalaScripts
{
    public const string InserirEscala = @"INSERT INTO Escalas (id_integrante, dt_data_escala, tipo_escala)
                                          VALUES (@IdIntegrante, @Data, @TipoEscala)";
    
    public const string ObterEscala = @"SELECT Escalas.id_integrante IdIntegrante, 
                                               Escalas.dt_data_escala Data,  
                                               Integrantes.nome Nome,
                                               Escalas.tipo_escala TipoEscala,
                                               Tipo_escala.Descricao DescricaoTipoEscala
                                        FROM Escalas
                                            INNER JOIN Integrantes ON Escalas.id_integrante = Integrantes.id_integrante
                                            LEFT JOIN Tipo_escala ON Escalas.tipo_escala = Tipo_escala.id_tipo_escala
                                        ORDER BY Escalas.dt_data_escala ASC";
    
    public const string ObterEscalaPorId = @"SELECT Escalas.id_integrante IdIntegrante, 
                                               Escalas.dt_data_escala Data,  
                                               Escalas.tipo_escala TipoEscala,
                                               Integrantes.nome Nome
                                        FROM Escalas
                                            INNER JOIN Integrantes ON Escalas.id_integrante = Integrantes.id_integrante
                                        WHERE Escalas.id_escala = @IdEscala
                                        ORDER BY Escalas.dt_data_escala ASC";

    public const string AtualizarEscala = @"UPDATE Escalas
                                        SET id_integrante = @IdEscala,
                                            dt_data_escala = @Data,
                                            tipo_escala = @TipoEscala,
                                        WHERE id_escala = @IdEscala
                                        SELECT @@ROWCOUNT AS LinhasAfetadas;";
}