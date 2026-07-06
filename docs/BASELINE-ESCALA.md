# Baseline — POST /escalas (legado)

Documentação do comportamento antes da refatoração PRD v1.1.

## Endpoint

`POST /escalas` com body `EscalaIntegrantes`:

```json
{
  "dataInicio": "2026-01-01",
  "dataFim": "2026-01-31",
  "tipoEscala": [1, 8],
  "diasDaSemana": [3, 0],
  "persistir": false
}
```

## Comportamento documentado

1. Valida datas, tipos e dias da semana.
2. Para cada dia da semana e cada tipo, busca integrantes elegíveis (tipo + disponibilidade).
3. Carrega **todo** o histórico de escalas do banco a cada iteração.
4. Seleciona integrante com menor contagem global `(integrante, tipo)`; desempate aleatório.
5. `persistir: false` retorna preview sem gravar; `true` grava no banco.
6. Slots já ocupados na mesma data+tipo são ignorados.

## Limitações conhecidas (motivo da refatoração)

- Contagem global injusta para disponibilidades diferentes.
- `new Random()` a cada iteração.
- Tipos fixos no seed musical.
- Sem configuração de escala persistente.
- Sem escolha de estratégia de algoritmo.

## Substituição (v2)

- `POST /configuracoes-escala` — define range, dias, tipos e estratégia.
- `POST /escalas/gerar` — gera preview com token.
- `POST /escalas/preview/{token}/persistir` — persiste o lote exibido.

O endpoint legado `POST /escalas` permanece até a migração completa (Passo 7).
