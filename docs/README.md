# EscalaApi — Plataforma Genérica de Escalas

API REST para gestão e geração automática de escalas, aplicável a qualquer contexto (plantões, voluntariado, equipes, etc.).

## Execução rápida

```bash
cd infra/docker
docker-compose up -d
```

- API: http://localhost:8010
- Swagger: http://localhost:8010

## Ordem de configuração

1. `POST /tipos-integrante` — cadastrar papéis/funções
2. `POST /integrantes` — cadastrar pessoas com tipos e disponibilidade
3. `GET /estrategias-algoritmo` — escolher critério de rotação
4. `POST /configuracoes-escala` — definir período, dias e tipos
5. `POST /escalas/gerar` — gerar preview (retorna token)
6. `POST /escalas/preview/{token}/persistir` — salvar escala

## Documentação

- [PRD](PRD.md) — requisitos completos
- [ALGORITMO-ROTACAO.md](ALGORITMO-ROTACAO.md) — especificação do algoritmo
- [BASELINE-ESCALA.md](BASELINE-ESCALA.md) — comportamento legado

## CI/CD

GitHub Actions em `.github/workflows/ci.yml`:
- Build e testes em push/PR para `main` e `feature/**`
- Build da imagem Docker

## Parâmetros do sistema

- `GET /parametros` — range máximo de datas (default: mensal)
- `PUT /parametros/range-maximo` — alterar limite

## Estratégias de algoritmo

| Estratégia | Quando usar |
|------------|-------------|
| Contextual por dia da semana | Disponibilidades diferentes entre integrantes |
| Contagem global | Carga total por pessoa é o critério |

---

Feito por Felipe
