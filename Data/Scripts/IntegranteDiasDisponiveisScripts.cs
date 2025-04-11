namespace EscalaApi.Data.Scripts;

public class IntegranteDiasDisponiveisScripts
{
    public const string InserirIntegranteDiasDisponiveis = @"INSERT INTO integrantes_dias_disponiveis 
                                                            (id_integrante, dia_disponivel)
                                                            VALUES 
                                                            (@IdIntegrante, @DiaDisponivel)";
   public const string RemoverIntegranteDiasDisponiveis = @"DELETE FROM integrantes_dias_disponiveis
                                                            WHERE id_integrante = @IdIntegrante";
}