-- Migration 007: Simplificação da geração de escalas
USE EscalaDb;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
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

    -- Remover default constraints antes de remover as colunas
    DECLARE @def_granularidade NVARCHAR(200);
    SELECT @def_granularidade = d.name
    FROM sys.default_constraints d
    JOIN sys.columns c ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
    WHERE d.parent_object_id = OBJECT_ID('configuracao_escala') AND c.name = 'id_tipo_granularidade';
    IF @def_granularidade IS NOT NULL
        EXEC('ALTER TABLE configuracao_escala DROP CONSTRAINT ' + @def_granularidade);

    DECLARE @def_imutavel NVARCHAR(200);
    SELECT @def_imutavel = d.name
    FROM sys.default_constraints d
    JOIN sys.columns c ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
    WHERE d.parent_object_id = OBJECT_ID('configuracao_escala') AND c.name = 'fl_estrategia_imutavel';
    IF @def_imutavel IS NOT NULL
        EXEC('ALTER TABLE configuracao_escala DROP CONSTRAINT ' + @def_imutavel);

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
