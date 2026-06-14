---
title: Sem interfaces e sem DI (júnior-friendly)
type: knowledge
area: architecture
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-sessao-decisoes.md
  - raw/sources/2026-06-14-padroes-plugin-comunidade.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [architecture, simplicidade]
---

# Sem interfaces e sem DI (júnior-friendly)

> Classes **concretas**; dependências montadas com `new` (ex.:
> `new ContaServico(new ContaRepositorio(ctx.UserService), new ContatoRepositorio(ctx.UserService))`).
> Sem framework de DI no sandbox.

## Contexto
Interfaces + composition root + casts pesavam para devs juniores e adicionavam arquivos.

## A decisão / conceito
- Regra trivial fica no plugin; extrai service só quando cresce/mexe em dados.
- **Por quê:** menos cerimônia, fácil de seguir; ainda testável (repo recebe `IOrganizationService` fake).

## Relacionadas
- [[vertical-slice-screaming]] · [[testes]]
