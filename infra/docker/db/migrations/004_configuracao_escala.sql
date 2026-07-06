-- Migration 004: Configuracao de escala
USE EscalaDb;
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '004_configuracao_escala.sql')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'configuracao_escala')
    BEGIN
        CREATE TABLE configuracao_escala (
            id_configuracao INT IDENTITY(1,1) PRIMARY KEY,
            desc_nome NVARCHAR(200) NOT NULL,
            dt_inicio DATE NOT NULL,
            dt_fim DATE NOT NULL,
            id_estrategia_algoritmo INT NOT NULL,
            id_tipo_granularidade INT NOT NULL DEFAULT 1,
            fl_estrategia_imutavel BIT NOT NULL DEFAULT 0,
            fl_ativo BIT NOT NULL DEFAULT 1,
            CONSTRAINT FK_config_estrategia FOREIGN KEY (id_estrategia_algoritmo) REFERENCES estrategia_algoritmo(id_estrategia_algoritmo),
            CONSTRAINT FK_config_granularidade FOREIGN KEY (id_tipo_granularidade) REFERENCES tipo_granularidade(id_tipo_granularidade)
        );
    END

    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'configuracao_escala_slot')
    BEGIN
        CREATE TABLE configuracao_escala_slot (
            id INT IDENTITY(1,1) PRIMARY KEY,
            id_configuracao INT NOT NULL,
            valor_slot INT NOT NULL,
            CONSTRAINT FK_config_slot FOREIGN KEY (id_configuracao) REFERENCES configuracao_escala(id_configuracao),
            CONSTRAINT UQ_config_slot UNIQUE (id_configuracao, valor_slot)
        );
    END

    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'configuracao_escala_tipo')
    BEGIN
        CREATE TABLE configuracao_escala_tipo (
            id INT IDENTITY(1,1) PRIMARY KEY,
            id_configuracao INT NOT NULL,
            id_tipo_integrante INT NOT NULL,
            CONSTRAINT FK_config_tipo_config FOREIGN KEY (id_configuracao) REFERENCES configuracao_escala(id_configuracao),
            CONSTRAINT FK_config_tipo_catalogo FOREIGN KEY (id_tipo_integrante) REFERENCES tipo_integrante_catalogo(id_tipo_integrante),
            CONSTRAINT UQ_config_tipo UNIQUE (id_configuracao, id_tipo_integrante)
        );
    END

    INSERT INTO schema_migrations (nome_arquivo) VALUES ('004_configuracao_escala.sql');
END
GO
