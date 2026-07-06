-- Migration 001: Registro de baseline do schema legado
USE EscalaDb;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'schema_migrations')
BEGIN
    CREATE TABLE schema_migrations (
        id_migration INT IDENTITY(1,1) PRIMARY KEY,
        nome_arquivo NVARCHAR(200) NOT NULL UNIQUE,
        dt_aplicacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '001_baseline.sql')
    INSERT INTO schema_migrations (nome_arquivo) VALUES ('001_baseline.sql');
GO
