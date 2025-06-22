namespace EscalaApi.Data.Scripts;

public class EscalaScripts
{
    public const string InserirEscala = @"INSERT INTO Escalas (id_integrante, dt_data_escala, cd_tipo_escala)
                                          VALUES (@IdIntegrante, @Data, @TipoEscala)";
    
    public const string ObterEscala = @"SELECT Escalas.id_escala IdEscala,
                                               Escalas.id_integrante IdIntegrante, 
                                               Escalas.dt_data_escala Data,  
                                               Integrantes.nome Nome,
                                               Escalas.cd_tipo_escala TipoEscala,
                                               cd_tipo_escala.Descricao DescricaoTipoEscala
                                        FROM Escalas
                                            INNER JOIN Integrantes ON Escalas.id_integrante = Integrantes.id_integrante
                                            LEFT JOIN cd_tipo_escala ON Escalas.cd_tipo_escala = cd_tipo_escala.id_cd_tipo_escala";
    
    public const string ObterEscalaPorId = @"SELECT Escalas.id_integrante IdIntegrante, 
                                               Escalas.dt_data_escala Data,  
                                               Escalas.cd_tipo_escala TipoEscala,
                                               Integrantes.nome Nome
                                        FROM Escalas
                                            INNER JOIN Integrantes ON Escalas.id_integrante = Integrantes.id_integrante
                                        WHERE Escalas.id_escala = @IdEscala
                                        ORDER BY Escalas.dt_data_escala ASC";

    public const string AtualizarEscala = @"UPDATE Escalas
                                        SET id_integrante = @IdEscala,
                                            dt_data_escala = @Data,
                                            cd_tipo_escala = @TipoEscala,
                                        WHERE id_escala = @IdEscala
                                        SELECT @@ROWCOUNT AS LinhasAfetadas;";
}