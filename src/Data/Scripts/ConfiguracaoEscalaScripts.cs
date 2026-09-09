namespace EscalaApi.Data.Scripts;

public static class ConfiguracaoEscalaScripts
{
    public const string Inserir = @"
        INSERT INTO configuracao_escala (desc_nome, dt_inicio, dt_fim)
        VALUES (@Nome, @DataInicio, @DataFim);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

    public const string ObterPorId = @"
        SELECT c.id_configuracao AS IdConfiguracao, c.desc_nome AS Nome,
               c.dt_inicio AS DataInicio, c.dt_fim AS DataFim,
               c.fl_ativo AS Ativo
        FROM configuracao_escala c
        WHERE c.id_configuracao = @Id";

    public const string Listar = @"
        SELECT c.id_configuracao AS IdConfiguracao, c.desc_nome AS Nome,
               c.dt_inicio AS DataInicio, c.dt_fim AS DataFim,
               c.fl_ativo AS Ativo
        FROM configuracao_escala c
        WHERE c.fl_ativo = 1 ORDER BY c.id_configuracao DESC";

    public const string InserirSlot = @"
        INSERT INTO configuracao_escala_slot (id_configuracao, valor_slot) VALUES (@IdConfiguracao, @ValorSlot)";

    public const string InserirTipo = @"
        INSERT INTO configuracao_escala_tipo (id_configuracao, id_tipo_integrante) VALUES (@IdConfiguracao, @IdTipo)";

    public const string ObterSlots = @"
        SELECT valor_slot FROM configuracao_escala_slot WHERE id_configuracao = @Id ORDER BY valor_slot";

    public const string ObterTipos = @"
        SELECT id_tipo_integrante FROM configuracao_escala_tipo WHERE id_configuracao = @Id";

    public const string Atualizar = @"
        UPDATE configuracao_escala SET desc_nome = @Nome, dt_inicio = @DataInicio, dt_fim = @DataFim
        WHERE id_configuracao = @Id";

    public const string ExcluirSlots = @"DELETE FROM configuracao_escala_slot WHERE id_configuracao = @Id";
    public const string ExcluirTipos = @"DELETE FROM configuracao_escala_tipo WHERE id_configuracao = @Id";
}
