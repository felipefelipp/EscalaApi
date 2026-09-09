-- Migration 007: Simplificação da geração de escalas
USE EscalaDb;
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '007_simplificacao_geracao.sql')
BEGIN
    -- 1. Remove tabela escala_preview e coluna id_preview_origem de escalas
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'escala_preview')
    BEGIN
        DROP TABLE escala_preview;
    END

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('escalas') AND name = 'id_preview_origem')
    BEGIN
        ALTER TABLE escalas DROP COLUMN id_preview_origem;
    END

    -- 2. Remove foreign keys e colunas de estrategia e granularidade de configuracao_escala
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_config_estrategia')
    BEGIN
        ALTER TABLE configuracao_escala DROP CONSTRAINT FK_config_estrategia;
    END

    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_config_granularidade')
    BEGIN
        ALTER TABLE configuracao_escala DROP CONSTRAINT FK_config_granularidade;
    END

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('configuracao_escala') AND name = 'id_estrategia_algoritmo')
    BEGIN
        ALTER TABLE configuracao_escala DROP COLUMN id_estrategia_algoritmo;
    END

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('configuracao_escala') AND name = 'id_tipo_granularidade')
    BEGIN
        ALTER TABLE configuracao_escala DROP COLUMN id_tipo_granularidade;
    END

    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('configuracao_escala') AND name = 'fl_estrategia_imutavel')
    BEGIN
        ALTER TABLE configuracao_escala DROP COLUMN fl_estrategia_imutavel;
    END

    -- 3. Remove tabelas de catálogo dinâmico que não são mais usadas
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_granularidade')
    BEGIN
        DROP TABLE tipo_granularidade;
    END

    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'estrategia_algoritmo')
    BEGIN
        DROP TABLE estrategia_algoritmo;
    END

    INSERT INTO schema_migrations (nome_arquivo) VALUES ('007_simplificacao_geracao.sql');
END
GO
