namespace EscalaApi.Data.Scripts;

public class EscalaScripts
{
    public const string InserirEscala = @"INSERT INTO escalas (id_integrante, dt_data_escala, cd_tipo_escala)
                                          VALUES (@IdIntegrante, @Data, @TipoEscala)";
    
    public const string  ObterEscala = @"SELECT escalas.id_escala IdEscala,
                                               escalas.id_integrante IdIntegrante, 
                                               escalas.dt_data_escala Data,  
                                               integrantes.desc_nome Nome,
                                               escalas.cd_tipo_escala TipoEscala,
                                               tipo_escala.txt_descricao DescricaoTipoEscala
                                        FROM escalas
                                            INNER JOIN integrantes ON escalas.id_integrante = integrantes.id_integrante
                                            LEFT JOIN tipo_escala ON escalas.cd_tipo_escala = tipo_escala.id_tipo_escala";
    
    public const string ObterEscalaPorId = @"SELECT escalas.id_escala IdEscala, 
                                               escalas.id_integrante IdIntegrante, 
                                               escalas.dt_data_escala Data,  
                                               escalas.cd_tipo_escala TipoEscala,
                                               integrantes.desc_nome Nome
                                        FROM escalas
                                            INNER JOIN integrantes ON escalas.id_integrante = integrantes.id_integrante
                                        WHERE escalas.id_escala = @IdEscala
                                        ORDER BY escalas.dt_data_escala ASC";

    public const string AtualizarEscala = @"UPDATE escalas
                                        SET id_integrante = @IdIntegrante,
                                            dt_data_escala = @Data,
                                            cd_tipo_escala = @TipoEscala
                                        WHERE id_escala = @IdEscala
                                        SELECT @@ROWCOUNT AS LinhasAfetadas;";
}