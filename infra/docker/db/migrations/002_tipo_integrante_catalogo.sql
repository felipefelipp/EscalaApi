-- Migration 002: Catálogo tipo_integrante + renomear join table
USE EscalaDb;
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '002_tipo_integrante_catalogo.sql')
BEGIN
    -- Renomear join table legada
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_integrante' AND type = 'U')
       AND NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'integrante_tipo')
    BEGIN
        EXEC sp_rename 'tipo_integrante', 'integrante_tipo';
    END

    -- Catálogo de tipos de integrante (papéis/funções)
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_integrante_catalogo')
    BEGIN
        CREATE TABLE tipo_integrante_catalogo (
            id_tipo_integrante INT IDENTITY(1,1) PRIMARY KEY,
            desc_nome NVARCHAR(100) NOT NULL UNIQUE,
            descricao NVARCHAR(500) NULL,
            dt_criacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
            fl_ativo BIT NOT NULL DEFAULT 1
        );
    END

    -- Migrar tipos legados de tipo_escala para catálogo (se existirem)
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_escala')
    BEGIN
        INSERT INTO tipo_integrante_catalogo (desc_nome, descricao)
        SELECT txt_descricao, 'Migrado de tipo_escala legado'
        FROM tipo_escala te
        WHERE NOT EXISTS (
            SELECT 1 FROM tipo_integrante_catalogo tic WHERE tic.desc_nome = te.txt_descricao
        );
    END

    INSERT INTO schema_migrations (nome_arquivo) VALUES ('002_tipo_integrante_catalogo.sql');
END
GO
