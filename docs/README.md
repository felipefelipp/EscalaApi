# 🎵 Gerenciador de Escalas Musicais - API

[![.NET](https://img.shields.io/badge/.NET-8.0-%23512bd4)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-✓-blue)](https://www.docker.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-%23CC2927)](https://www.microsoft.com/sql-server)

API para gerenciamento de escalas, músicos e eventos para grupos musicais com Docker.

## 🚀 Execução Rápida

**Pré-requisito único**:  
✅ Docker instalado ([Download Docker](https://www.docker.com/products/docker-desktop))

```bash
git clone https://github.com/felipefelipp/EscalaApi.git
cd infra/docker
docker-compose up -d
```

**Acesse**:
- API: http://localhost:8010/swagger
- SQL Server: `localhost,1433` (usuário: `sa`, senha: `escalaApi34@FF`)

## 🔌 Portas do Projeto

| Serviço       | Porta  | Acesso                         |
|---------------|--------|--------------------------------|
| API .NET      | 8010   | http://localhost:8010          |
| SQL Server    | 1433   | localhost,1433                 |

## 📚 Documentação da API

Acesse via Swagger UI após iniciar os containers:  
http://localhost:8010/swagger

### Principais Funcionalidades:
- Cadastro de músicos e instrumentos
- Geração automática de escalas
- Disponibilidade de músicos

## 🛠 Comandos Úteis

| Comando                       | Descrição                    |
|-------------------------------|------------------------------|
| `docker-compose up -d`        | Inicia todos os serviços     |
| `docker-compose down`         | Para e remove os containers  |
| `docker-compose logs -f api`  | Visualiza logs em tempo real |

## 📊 Estrutura do Banco

O container do SQL Server é automaticamente configurado com:
- Banco de dados: `EscalaDb`
- Usuário: `sa`
- Senha: `escalaApi34@FF`

```sql
-- Exemplo de conexão via SQLCMD:
docker exec -it db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P escalaApi34@FF -d EscalaDb
```

---

Feito com ❤️ por Felipe - ✉️ [lipesantos.gtt@gmail.com](mailto:lipesantos.gtt@gmail.com)
```

Principais melhorias:
1. **Seção de execução rápida** destacada no topo
2. **Tabela clara das portas** utilizadas
3. **Requisito simplificado** - apenas Docker necessário
4. **Comandos essenciais** destacados
5. **Informações de conexão** ao banco mais visíveis
6. Removidas seções menos relevantes para focar no essencial

Você pode adicionar/remover seções conforme necessidade específica do seu projeto.