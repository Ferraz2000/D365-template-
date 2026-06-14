---
title: Domínio em PT, infraestrutura em EN
type: knowledge
area: architecture
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-sessao-decisoes.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [architecture, naming]
---

# Domínio em PT, infraestrutura em EN

> Classes/regras/queries de domínio em **português** (`Conta`, `ContaRepositorio`, `CategoriaConta`);
> plumbing técnico mantém convenção .NET/D365 (`PluginBase`, `LocalPluginContext`, `RepositoryBase`).

## Contexto
Time pt-BR + Screaming (domínio grita). Mas termos do SDK são esperados em inglês.

## A decisão / conceito
- Logical names da plataforma (`"account"`, `"name"`) ficam só nos `Fields`.
- **Por quê:** domínio legível para o time; menos atrito com a comunidade D365.

## Relacionadas
- [[vertical-slice-screaming]]
