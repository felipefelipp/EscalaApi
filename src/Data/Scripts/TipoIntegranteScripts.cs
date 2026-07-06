namespace EscalaApi.Data.Scripts;

public class TipoIntegranteScripts
{
    public const string InserirTipoIntegrante = @"INSERT INTO integrante_tipo (id_integrante, cd_tipo_integrante)
                                                   VALUES (@IdIntegrante, @TipoIntegrante)";
    public const string ExcluirTipoIntegrante = @"DELETE FROM integrante_tipo WHERE id_integrante = @IdIntegrante";
}