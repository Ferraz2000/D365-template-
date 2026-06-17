---
title: Vertical slice (Screaming) por feature
type: knowledge
area: architecture
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-padroes-plugin-comunidade.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [architecture, screaming]
---

# Vertical slice (Screaming) por feature

> O assembly é organizado **por feature** (namespace = feature): `Contas/`, `Contatos/`.
> O topo grita o domínio. Infra compartilhada em `Common/` e `Integracao/`.

## Contexto
Organizar por camada técnica (Model/Services/Repositories) não grita o domínio e mistura entidades.

## A decisão / conceito
- Cada feature reúne model + repositório + service + plugins no mesmo namespace.
- **Regra de dependência:** `Plugin → Service → Repositório → IOrganizationService`;
  feature depende numa direção (`Contas → Contatos`); `Common`/`Integracao` não dependem de feature.
- **Por quê:** descoberta rápida, escala por feature; trade-off: separação de camadas vira convenção
  garantida pelos **testes de arquitetura**.

## Relacionadas
- [[dominio-pt-infra-en]] · [[sem-interfaces-sem-di]] · [[padroes-de-plugin]]
