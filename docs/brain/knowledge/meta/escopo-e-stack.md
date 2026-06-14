---
title: Escopo e stack do template D365 CE
type: knowledge
area: meta
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-sessao-decisoes.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [meta, escopo]
---

# Escopo e stack do template

> Template para **D365 Customer Engagement** (Dataverse): plugins C# (net462), web resources
> TypeScript e PCF. Repo guarda **só código-fonte + o padrão**; solutions exportadas ficam fora.

## Contexto
Solutions unpacked pesam e poluem o histórico. O time trabalha por **PR** com **doc-sync gate**.

## A decisão / conceito
- Idioma de trabalho **pt-BR**; **domínio em PT, infra em EN**.
- `team=true`: PR + sugestão de branch protegido para o vault.
- Memória durável no Hipocampo (`docs/brain/`); padrão escrito em `docs/architecture/`.

## Relacionadas
- [[vertical-slice-screaming]] · [[solutions-fora-do-repo]]
