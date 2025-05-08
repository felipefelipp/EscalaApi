-- Cria o banco de dados se não existir
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'EscalaDb')
BEGIN
    CREATE DATABASE EscalaDb;
    PRINT 'Banco de dados EscalaDb criado com sucesso.';
END
ELSE
BEGIN
    PRINT 'Banco de dados EscalaDb já existe.';
END
GO

-- Usa o banco de dados EscalaDb
USE EscalaDb;
GO

-- Criação da tabela Escalas se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Escalas' AND type = 'U')
BEGIN
    CREATE TABLE Escalas (
        id_escala INT IDENTITY(1,1),
        id_integrante INT,
        dt_data_escala NVARCHAR(50), -- Use DATE ou DATETIME2 para datas reais
        tipo_escala INT,
        PRIMARY KEY(id_escala)
    );
    PRINT 'Tabela Escalas criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela Escalas já existe.';
END
GO

-- Criação da tabela Integrantes se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Integrantes' AND type = 'U')
BEGIN
    CREATE TABLE Integrantes (
        id_integrante INT IDENTITY(1,1),
        nome NVARCHAR(100), -- NVARCHAR é melhor para texto no SQL Server
        PRIMARY KEY(id_integrante)
    );
    PRINT 'Tabela Integrantes criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela Integrantes já existe.';
END
GO

-- Criação da tabela Integrantes_dias_disponiveis se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Integrantes_dias_disponiveis' AND type = 'U')
BEGIN
    CREATE TABLE Integrantes_dias_disponiveis (
        id_integrante_dias_disponiveis INT IDENTITY(1,1),
        id_integrante INT,
        dia_disponivel INT,
        PRIMARY KEY(id_integrante_dias_disponiveis)
    );
    PRINT 'Tabela Integrantes_dias_disponiveis criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela Integrantes_dias_disponiveis já existe.';
END
GO

-- Criação da tabela Tipo_integrante se não existir
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tipo_integrante' AND type = 'U')
BEGIN
    CREATE TABLE Tipo_integrante (
        id_tipo_integrante INT IDENTITY(1,1),
        id_integrante INT,
        tipo_integrante INT,
        PRIMARY KEY(id_tipo_integrante)
    );
    PRINT 'Tabela Tipo_integrante criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela Tipo_integrante já existe.';
END
GO

-- Adiciona mensagem final
PRINT 'Script de inicialização do banco de dados concluído com sucesso.';
GO