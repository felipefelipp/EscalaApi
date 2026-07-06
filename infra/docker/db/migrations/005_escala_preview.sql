-- Migration 005: Preview de escala
USE EscalaDb;
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '005_escala_preview.sql')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'escala_preview')
    BEGIN
        CREATE TABLE escala_preview (
            id_preview INT IDENTITY(1,1) PRIMARY KEY,
            token VARCHAR(500) NOT NULL UNIQUE,
            id_configuracao INT NOT NULL,
            payload_json NVARCHAR(MAX) NOT NULL,
            dt_criacao DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
            dt_expiracao DATETIME2 NOT NULL,
            fl_persistido BIT NOT NULL DEFAULT 0,
            CONSTRAINT FK_preview_config FOREIGN KEY (id_configuracao) REFERENCES configuracao_escala(id_configuracao)
        );
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('escalas') AND name = 'id_configuracao')
        ALTER TABLE escalas ADD id_configuracao INT NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('escalas') AND name = 'id_preview_origem')
        ALTER TABLE escalas ADD id_preview_origem INT NULL;

    INSERT INTO schema_migrations (nome_arquivo) VALUES ('005_escala_preview.sql');
END
GO
