---
title: Padrões de plugin (1 step, early-bound, gatilhos)
type: knowledge
area: plugins
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-padroes-plugin-comunidade.md
  - raw/sources/2026-06-14-sessao-decisoes.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [plugins, padrao]
---

# Padrões de plugin

> **1 plugin = 1 responsabilidade = 1 step.** Herde de `PluginBase` e implemente `Execute(context)`.
> Entidades **tipadas** (early-bound). Regra no service, **query no repositório**.

## Contexto
Acoplar várias ações de uma tabela numa classe (switch de mensagem/estágio) é anti-padrão.

## A decisão / conceito
- Gatilhos cobertos: Pre-Validation (validação), Pre/Post-Operation, Post+PreImage,
  **Custom Message** (Custom API) e **anti-loop** com `context.Depth > 1`.
- Tipos do D365 expostos tipados: Money, OptionSet, multi-select, state, lookup, datetime.
- **Por quê:** cada unidade isolada, testável e legível.

## Relacionadas
- [[integracoes]] · [[testes]] · [[vertical-slice-screaming]]
