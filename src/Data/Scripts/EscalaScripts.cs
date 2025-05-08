namespace EscalaApi.Data.Scripts;

public class EscalaScripts
{
    public const string InserirEscala = @"INSERT INTO Escalas (id_integrante, dt_data_escala, tipo_escala)
                                          VALUES (@IdIntegrante, @Data, @TipoEscala)";
    
    public const string ObterEscala = @"SELECT Escalas.id_integrante IdIntegrante, 
                                               Escalas.dt_data_escala Data,  
                                               Escalas.tipo_escala TipoEscala,
                                               Integrantes.nome Nome
                                        FROM Escalas
                                            INNER JOIN Integrantes ON Escalas.id_integrante = Integrantes.id_integrante
                                        ORDER BY Escalas.dt_data_escala ASC";
}