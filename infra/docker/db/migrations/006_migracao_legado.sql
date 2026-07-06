-- Migration 006: Integridade e migracao legado
USE EscalaDb;
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '006_migracao_legado.sql')
BEGIN
    -- Adicionar id_tipo_integrante em escalas se nao existir
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('escalas') AND name = 'id_tipo_integrante')
    BEGIN
        ALTER TABLE escalas ADD id_tipo_integrante INT NULL;
        UPDATE e SET e.id_tipo_integrante = tic.id_tipo_integrante
        FROM escalas e
        INNER JOIN tipo_escala te ON e.cd_tipo_escala = te.id_tipo_escala
        INNER JOIN tipo_integrante_catalogo tic ON tic.desc_nome = te.txt_descricao
        WHERE e.id_tipo_integrante IS NULL;
    END

    -- UNIQUE constraint em data+tipo (via id_tipo_integrante ou cd_tipo_escala)
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_escala_data_tipo' AND object_id = OBJECT_ID('escalas'))
    BEGIN
        CREATE UNIQUE INDEX UQ_escala_data_tipo ON escalas (dt_data_escala, cd_tipo_escala)
        WHERE cd_tipo_escala IS NOT NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_escalas_config_tipo_data')
        CREATE INDEX IX_escalas_config_tipo_data ON escalas (id_configuracao, cd_tipo_escala, dt_data_escala);

    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_escalas_integrante_tipo')
        CREATE INDEX IX_escalas_integrante_tipo ON escalas (id_integrante, cd_tipo_escala, dt_data_escala);

    INSERT INTO schema_migrations (nome_arquivo) VALUES ('006_migracao_legado.sql');
END
GO
