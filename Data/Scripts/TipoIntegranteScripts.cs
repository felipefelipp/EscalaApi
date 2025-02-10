namespace EscalaApi.Data.Scripts;

public class TipoIntegranteScripts
{
    public const string InserirTipoIntegrante = @"INSERT INTO Tipo_integrante (id_integrante, tipo_integrante)
                                                   VALUES (@IdIntegrante, @TipoIntegrante)";
}