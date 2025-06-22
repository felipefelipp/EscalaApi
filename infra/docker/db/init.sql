IF DB_ID('EscalaDb') IS NULL
BEGIN
    CREATE DATABASE EscalaDb;
    PRINT 'Banco de dados EscalaDb criado com sucesso.';
END
GO

-- Usa o banco de dados EscalaDb
USE EscalaDb;
GO

-- Criação da tabela escalas se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'escalas' AND type = 'U')
BEGIN
    CREATE TABLE escalas (
        id_escala INT IDENTITY(1,1),
        id_integrante INT,
        dt_data_escala NVARCHAR(50), 
        cd_tipo_escala INT,
        PRIMARY KEY(id_escala)
    );
    PRINT 'Tabela escalas criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela escalas já existe.';
END
GO

-- Criação da tabela integrantes se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'integrantes' AND type = 'U')
BEGIN
    CREATE TABLE integrantes (
        id_integrante INT IDENTITY(1,1),
        desc_nome NVARCHAR(100), 
        PRIMARY KEY(id_integrante)
    );
    PRINT 'Tabela integrantes criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela integrantes já existe.';
END
GO

-- Criação da tabela integrantes_dias_disponiveis se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'integrantes_dias_disponiveis' AND type = 'U')
BEGIN
    CREATE TABLE integrantes_dias_disponiveis (
        id_integrante_dias_disponiveis INT IDENTITY(1,1),
        id_integrante INT,
        cd_dia_disponivel INT,
        PRIMARY KEY(id_integrante_dias_disponiveis)
    );
    PRINT 'Tabela integrantes_dias_disponiveis criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela integrantes_dias_disponiveis já existe.';
END
GO

-- Criação da tabela tipo_integrante se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_integrante' AND type = 'U')
BEGIN
    CREATE TABLE tipo_integrante (
        id_tipo_integrante INT IDENTITY(1,1),
        id_integrante INT,
        cd_tipo_integrante INT,
        PRIMARY KEY(id_tipo_integrante)
    );
    PRINT 'Tabela tipo_integrante criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela tipo_integrante já existe.';
END
GO

-- Criação da tabela tipo_escala se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tipo_escala' AND type = 'U')
BEGIN
    CREATE TABLE tipo_escala (
	id_tipo_escala int IDENTITY(1,1) NOT NULL,
	txt_descricao varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
    );
    PRINT 'Tabela tipo_escala criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela tipo_escala já existe.';
END
GO

USE EscalaDb;

IF (SELECT COUNT(*) FROM tipo_escala) = 0
BEGIN
    SET IDENTITY_INSERT tipo_escala ON;
    INSERT INTO tipo_escala
        (id_tipo_escala, txt_descricao)
    VALUES
        (1, 'Ministro'),
        (2, 'BackingVocal'),
        (3, 'BackingVocal2'),
        (4, 'Teclado'),
        (5, 'Violao'),
        (6, 'ContraBaixo'),
        (7, 'Guitarra'),
        (8, 'Bateria');
    SET IDENTITY_INSERT tipo_escala OFF;
    PRINT 'valores de tipo_escala inseridos com sucesso.';
END
ELSE
BEGIN
    PRINT 'dados já existem.';
END
GO

-- Adiciona mensagem final
PRINT 'Script de inicialização do banco de dados concluído com sucesso.';
GO