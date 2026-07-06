namespace EscalaApi.Data.Scripts;

public static class ConfiguracaoEscalaScripts
{
    public const string Inserir = @"
        INSERT INTO configuracao_escala (desc_nome, dt_inicio, dt_fim, id_estrategia_algoritmo, id_tipo_granularidade)
        VALUES (@Nome, @DataInicio, @DataFim, @IdEstrategia, @IdGranularidade);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

    public const string ObterPorId = @"
        SELECT c.id_configuracao AS IdConfiguracao, c.desc_nome AS Nome,
               c.dt_inicio AS DataInicio, c.dt_fim AS DataFim,
               c.id_estrategia_algoritmo AS IdEstrategiaAlgoritmo,
               e.codigo AS CodigoEstrategia,
               c.id_tipo_granularidade AS IdTipoGranularidade,
               c.fl_estrategia_imutavel AS EstrategiaImutavel, c.fl_ativo AS Ativo
        FROM configuracao_escala c
        INNER JOIN estrategia_algoritmo e ON c.id_estrategia_algoritmo = e.id_estrategia_algoritmo
        WHERE c.id_configuracao = @Id";

    public const string Listar = @"
        SELECT c.id_configuracao AS IdConfiguracao, c.desc_nome AS Nome,
               c.dt_inicio AS DataInicio, c.dt_fim AS DataFim,
               c.id_estrategia_algoritmo AS IdEstrategiaAlgoritmo,
               e.codigo AS CodigoEstrategia,
               c.id_tipo_granularidade AS IdTipoGranularidade,
               c.fl_estrategia_imutavel AS EstrategiaImutavel, c.fl_ativo AS Ativo
        FROM configuracao_escala c
        INNER JOIN estrategia_algoritmo e ON c.id_estrategia_algoritmo = e.id_estrategia_algoritmo
        WHERE c.fl_ativo = 1 ORDER BY c.id_configuracao DESC";

    public const string InserirSlot = @"
        INSERT INTO configuracao_escala_slot (id_configuracao, valor_slot) VALUES (@IdConfiguracao, @ValorSlot)";

    public const string InserirTipo = @"
        INSERT INTO configuracao_escala_tipo (id_configuracao, id_tipo_integrante) VALUES (@IdConfiguracao, @IdTipo)";

    public const string ObterSlots = @"
        SELECT valor_slot FROM configuracao_escala_slot WHERE id_configuracao = @Id ORDER BY valor_slot";

    public const string ObterTipos = @"
        SELECT id_tipo_integrante FROM configuracao_escala_tipo WHERE id_configuracao = @Id";

    public const string MarcarEstrategiaImutavel = @"
        UPDATE configuracao_escala SET fl_estrategia_imutavel = 1 WHERE id_configuracao = @Id";

    public const string Atualizar = @"
        UPDATE configuracao_escala SET desc_nome = @Nome, dt_inicio = @DataInicio, dt_fim = @DataFim,
               id_estrategia_algoritmo = @IdEstrategia, id_tipo_granularidade = @IdGranularidade
        WHERE id_configuracao = @Id";

    public const string ExcluirSlots = @"DELETE FROM configuracao_escala_slot WHERE id_configuracao = @Id";
    public const string ExcluirTipos = @"DELETE FROM configuracao_escala_tipo WHERE id_configuracao = @Id";
}
