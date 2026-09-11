# EscalaApi — Plataforma Genérica de Escalas

Plataforma completa para gestão e geração automática de escalas inteligentes, aplicável a qualquer contexto (plantões, voluntariado, ministérios musicais, equipes de atendimento, etc.).

A solução conta com:
- **Backend**: API REST em **.NET 10** com algoritmos de balanceamento justo de carga.
- **Frontend**: Single Page Application (SPA) em **Blazor WebAssembly (.NET 10)** com componentes visuais modernos via **MudBlazor**.
- **Banco de Dados**: **Microsoft SQL Server 2022** com scripts automáticos de inicialização e migração.
- **Orquestração**: Suporte completo a **Docker** e **Docker Compose** com hot-reload em ambiente de desenvolvimento.

---

## Sumário

- [Pré-requisitos](#pré-requisitos)
- [Como Rodar com Docker (Recomendado)](#como-rodar-com-docker-recomendado)
  - [Subindo a aplicação completa](#subindo-a-aplicação-completa)
  - [URLs e portas dos serviços](#urls-e-portas-dos-serviços)
  - [Comandos úteis do Docker](#comandos-úteis-do-docker)
- [Como Rodar Localmente (.NET CLI)](#como-rodar-localmente-net-cli)
  - [Passo 1: Iniciar o banco de dados](#passo-1-iniciar-o-banco-de-dados)
  - [Passo 2: Iniciar a API](#passo-2-iniciar-a-api)
  - [Passo 3: Iniciar o Frontend Web](#passo-3-iniciar-o-frontend-web)
- [Executando os Testes Automatizados](#executando-os-testes-automatizados)
- [Fluxo de Uso e Configuração](#fluxo-de-uso-e-configuração)
- [Parâmetros do Sistema](#parâmetros-do-sistema)
- [Documentação Complementar](#documentação-complementar)
- [CI/CD](#cicd)

---

## Pré-requisitos

### Opção 1: Execução via Docker (Recomendado)
- [Docker](https://www.docker.com/) (versão 24+ recomendada)
- [Docker Compose](https://docs.docker.com/compose/) (versão 2+)

### Opção 2: Execução Local (.NET CLI)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Docker Desktop (necessário para rodar o contêiner do SQL Server) ou uma instância local de SQL Server 2022.

---

## Como Rodar com Docker (Recomendado)

### Subindo a aplicação completa

A partir da **raiz do repositório**, execute:

```bash
docker compose -f infra/docker/docker-compose.yml up --build
```

Caso queira executar em segundo plano (*detached mode*):

```bash
docker compose -f infra/docker/docker-compose.yml up --build -d
```

> **Dica**: Você também pode entrar no diretório `infra/docker` e executar diretamente:
> ```bash
> cd infra/docker
> docker compose up --build -d
> ```

O Docker Compose irá inicializar automaticamente:
1. O banco **SQL Server 2022**, executando as migrações SQL em `infra/docker/db/migrations/`.
2. A **API REST** (`EscalaApi`), aguardando o banco ficar saudável (*healthcheck* ativo).
3. O **Frontend Web** (`EscalaApi.Web`), compilado e servido via Nginx Alpine de alta performance.

---

### URLs e portas dos serviços

| Serviço | Endereço | Descrição |
|---|---|---|
| 🌐 **Frontend Web** | [http://localhost:8020](http://localhost:8020) | Interface gráfica em Blazor WebAssembly + MudBlazor |
| 🚀 **Swagger UI** | [http://localhost:8010](http://localhost:8010) | Documentação interativa da API (redireciona raiz e `/swagger`) |
| 🔌 **API REST Base** | [http://localhost:8010](http://localhost:8010) | Endpoints REST da aplicação |
| 🗄️ **SQL Server 2022** | `localhost:1433` | Host do banco de dados (porta exposta) |

#### Credenciais do Banco de Dados:
- **Host**: `localhost,1433` (ou `sql-server` dentro da rede Docker)
- **Database**: `EscalaDb`
- **Usuário**: `sa`
- **Senha**: `escalaApi34@FF`

---

### Comandos úteis do Docker

- **Parar todos os serviços:**
  ```bash
  docker compose -f infra/docker/docker-compose.yml down
  ```

- **Resetar banco de dados (apaga volumes persistidos e recria do zero):**
  ```bash
  docker compose -f infra/docker/docker-compose.yml down -v
  docker compose -f infra/docker/docker-compose.yml up --build -d
  ```

- **Ver logs dos serviços:**
  ```bash
  # Todos os serviços
  docker compose -f infra/docker/docker-compose.yml logs -f

  # Apenas a API
  docker compose -f infra/docker/docker-compose.yml logs -f escala-api

  # Apenas o Frontend
  docker compose -f infra/docker/docker-compose.yml logs -f escala-web

  # Apenas o SQL Server
  docker compose -f infra/docker/docker-compose.yml logs -f sql-server
  ```

---

## Como Rodar Localmente (.NET CLI)

Se você preferir rodar a API e o Frontend na sua máquina física via terminal, utilize o contêiner apenas para o banco de dados:

### Passo 1: Iniciar o banco de dados
Suba apenas o serviço `sql-server` via Docker:
```bash
docker compose -f infra/docker/docker-compose.yml up -d sql-server
```
*(Aguarde alguns segundos até o healthcheck e as migrações serem concluídas)*.

### Passo 2: Iniciar a API
A partir da raiz do repositório:
```bash
dotnet run --project src/EscalaApi.csproj
```
- A API estará disponível em: [http://localhost:8010](http://localhost:8010)
- O Swagger pode ser acessado em: [http://localhost:8010](http://localhost:8010) ou [http://localhost:8010/swagger](http://localhost:8010/swagger)

### Passo 3: Iniciar o Frontend Web
Em um segundo terminal, a partir da raiz do repositório:
```bash
dotnet run --project src/EscalaApi.Web/EscalaApi.Web.csproj
```
- A interface web estará disponível em: [http://localhost:5177](http://localhost:5177) (ou [https://localhost:7271](https://localhost:7271))

---

## Executando os Testes Automatizados

Para rodar todos os testes unitários da solução:

```bash
dotnet test EscalaApi.sln
```

Ou executar apenas o projeto de testes diretamente:

```bash
dotnet test tests/EscalaApi.Tests/EscalaApi.Tests.csproj
```

---

## Fluxo de Uso e Configuração

Tanto pela Interface Web ([http://localhost:8020](http://localhost:8020)) quanto via chamadas à API REST ([http://localhost:8010](http://localhost:8010)), a sequência recomendada de configuração é:

1. **Tipos de Integrante** (`POST /tipos-integrante`):
   - Cadastrar os papéis/funções da escala (ex.: *Vocal*, *Violão*, *Médico*, *Atendente*).
2. **Integrantes** (`POST /integrantes`):
   - Cadastrar pessoas, atribuindo um ou mais tipos e definindo os dias da semana em que estão disponíveis (`diasDisponiveis`: `0` = Domingo a `6` = Sábado).
3. **Configurações de Escala** (`POST /configuracoes-escala`):
   - Definir período (`dataInicio` e `dataFim`), dias da semana recorrentes em que haverá escala (`valoresRecorrentes`) e os tipos de integrantes necessários para cada dia, incluindo a quantidade de vagas.
4. **Gerar Escala** (`POST /escalas/gerar`):
   - `persistir: false` — gera simulação (preview) direto no JSON da resposta com relatório de balanceamento e distribuição de carga.
   - `persistir: true` — gera e persiste a escala diretamente no banco de dados.

---

## Parâmetros do Sistema

- `GET /parametros`: Exibe as configurações do sistema, como o range máximo de datas (padrão: mensal).
- `PUT /parametros/range-maximo`: Permite alterar o limite permitido de geração de escalas.

---

## Documentação Complementar

- [PRD.md](docs/PRD.md) — Documento de Requisitos do Produto (visão funcional completa).
- [ALGORITMO-ROTACAO.md](docs/ALGORITMO-ROTACAO.md) — Especificação do algoritmo de rotação e critérios de desempate.
- [BASELINE-ESCALA.md](docs/BASELINE-ESCALA.md) — Comportamento e histórico legado da escala.

---

## CI/CD

O repositório possui pipeline automatizado configurado no GitHub Actions em `.github/workflows/ci.yml`:
- Compilação e execução de testes em .NET 10 para branches `main` e `feature/**`.
- Validação e build da imagem Docker de produção.

---

Feito por Felipe
