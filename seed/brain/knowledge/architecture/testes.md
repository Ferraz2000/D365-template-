---
title: Testes: harness próprio, net462 via Mono, arquitetura
type: knowledge
area: architecture
status: active
confidence: high
provenance: inferred
sources:
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [architecture, testes]
---

# Testes: harness próprio, net462 via Mono, arquitetura

> Suíte C# usa um **harness de fakes no repo** (sem FakeXrmEasy) — roda em net462/Mono/CI.
> Há **testes de arquitetura** (reflection) que enforçam a regra de dependência.

## Contexto
FakeXrmEasy **crasha o Mono** (reflection/Expression); além disso v2/v3 exigem licença.

## A decisão / conceito
- `FakeOrganizationService` em memória simula CRUD, query (filtros/ordem/multi-select), link-entity, N:N e UpdateRequest (concorrência).
- net462 é só Windows/Mono; em Linux o **Mono** executa a suíte.
- **Por quê:** roda em qualquer lugar, sem licença; FakeXrmEasy fica como opção documentada.

## Relacionadas
- [[sem-interfaces-sem-di]] · [[padroes-de-plugin]]
