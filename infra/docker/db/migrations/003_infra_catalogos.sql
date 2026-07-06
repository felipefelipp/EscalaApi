-- Migration 003: Catálogos de infraestrutura
USE EscalaDb;
GO

IF NOT EXISTS (SELECT 1 FROM schema_migrations WHERE nome_arquivo = '003_infra_catalogos.sql')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'estrategia_algoritmo')
    BEGIN
        CREATE TABLE estrategia_algoritmo (
            id_estrategia_algoritmo INT IDENTITY(1,1) PRIMARY KEY,
            codigo VARCHAR(50) NOT NULL UNIQUE,
            txt_nome NVARCHAR(100) NOT NULL,
            txt_descricao_detalhada NVARCHAR(MAX) NOT NULL,
            fl_ativo BIT NOT NULL DEFAULT 1
        );
    END

    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_granularidade')
    BEGIN
        CREATE TABLE tipo_granularidade (
            id_tipo_granularidade INT IDENTITY(1,1) PRIMARY KEY,
            codigo VARCHAR(50) NOT NULL UNIQUE,
            txt_nome NVARCHAR(100) NOT NULL,
            txt_descricao NVARCHAR(500) NULL,
            fl_ativo BIT NOT NULL DEFAULT 1
        );
    END

    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'parametro_sistema')
    BEGIN
        CREATE TABLE parametro_sistema (
            id_parametro INT IDENTITY(1,1) PRIMARY KEY,
            chave VARCHAR(100) NOT NULL UNIQUE,
            valor NVARCHAR(200) NOT NULL,
            descricao NVARCHAR(500) NULL,
            dt_atualizacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
        );
    END

    IF NOT EXISTS (SELECT 1 FROM estrategia_algoritmo WHERE codigo = 'contextual_dia_semana')
    BEGIN
        INSERT INTO estrategia_algoritmo (codigo, txt_nome, txt_descricao_detalhada, fl_ativo) VALUES
        ('contextual_dia_semana', 'Contextual por dia da semana',
         'Compara integrantes apenas dentro do mesmo dia da semana e tipo. A escala de quarta nao penaliza na selecao de domingo.',
         1),
        ('global', 'Contagem global',
         'Compara pelo total de escalas do integrante naquele tipo, independente do dia.',
         1);
    END

    IF NOT EXISTS (SELECT 1 FROM tipo_granularidade WHERE codigo = 'dias_semana')
    BEGIN
        INSERT INTO tipo_granularidade (codigo, txt_nome, txt_descricao, fl_ativo) VALUES
        ('dias_semana', 'Dias da semana', 'Escala por dias da semana (Domingo a Sabado).', 1),
        ('horas_dia', 'Horas do dia', 'Reservado para versao futura.', 0);
    END

    IF NOT EXISTS (SELECT 1 FROM parametro_sistema WHERE chave = 'range_maximo_escala')
    BEGIN
        INSERT INTO parametro_sistema (chave, valor, descricao) VALUES
        ('range_maximo_escala', 'mensal', 'Range maximo: semanal, mensal, trimestral, semestral, anual, ilimitado');
    END

    IF NOT EXISTS (SELECT 1 FROM parametro_sistema WHERE chave = 'preview_expiracao_horas')
    BEGIN
        INSERT INTO parametro_sistema (chave, valor, descricao) VALUES
        ('preview_expiracao_horas', '24', 'Horas ate expiracao do token de preview');
    END

    INSERT INTO schema_migrations (nome_arquivo) VALUES ('003_infra_catalogos.sql');
END
GO
