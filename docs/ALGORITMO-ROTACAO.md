# Algoritmo de Rotação — EscalaApi

Especificação técnica complementar ao [PRD](PRD.md).

## Visão geral

O módulo `Services/Rotacao/` gera escalas de forma determinística, com duas estratégias selecionáveis na configuração:

| ID | Código | Descrição |
|----|--------|-----------|
| 1 | `contextual_dia_semana` | Contagem por `(tipo, dia_da_semana)` |
| 2 | `global` | Contagem total por tipo |

## Fluxo

1. Carregar configuração de escala (range, slots, tipos, estratégia).
2. Expandir datas: `ExpansorDeDatas.Expand(inicio, fim, dias)`.
3. Carregar histórico filtrado por `id_configuracao`.
4. Para cada `(data, tipo)` sem slot ocupado:
   - Montar pool de candidatos (tipo + disponibilidade).
   - Calcular contagem via estratégia.
   - Desempate determinístico.
5. Retornar preview com token (24h default).

## Desempate (determinístico)

1. Menor contagem (conforme estratégia).
2. Integrante escalado há mais tempo no mesmo contexto.
3. Menor `id_integrante`.

## Estratégia 1 — Contextual

Ao selecionar para **Domingo**, conta apenas escalas de **Domingo** daquele tipo.

**Cenário João/Maria:**
- João: Qua+Dom | Maria: Dom
- Após 1 Qua(João) + 1 Dom(João) + 1 Dom(Maria), próximo Dom emparelha ambos.

## Estratégia 2 — Global

Conta todas as escalas do tipo, independente do dia. Maria é favorecida no domingo se João tem mais escalas totais.

## Preview e persistência

- `POST /escalas/gerar` → `previewToken` + lote em `escala_preview`.
- `POST /escalas/preview/{token}/persistir` → grava exatamente o lote.
- Novo preview invalida tokens anteriores da mesma configuração.

## Testes

- `ExpansorDeDatasTests`
- `CenarioJoaoMariaEstrategia1Tests`
- `CenarioJoaoMariaEstrategia2Tests`
