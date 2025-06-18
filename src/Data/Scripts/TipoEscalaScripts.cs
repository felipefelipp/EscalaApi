namespace EscalaApi.Data.Scripts;

public static class TipoEscalaScripts
{
    public const string ObterTiposEscalaDisponiveis = @"
        SELECT id_tipo_escala
        FROM Tipo_escala";
}