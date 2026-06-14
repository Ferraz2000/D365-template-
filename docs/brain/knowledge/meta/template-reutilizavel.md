---
title: Template reutilizável (dotnet new) + MIT/público
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
tags: [meta, template, licenca]
---

# Template reutilizável

> Distribuído como **`dotnet new` template** e/ou **GitHub Template**, MIT, público — e **sem injetar nada na org**.

## A decisão / conceito
- **`dotnet new d365ce -n Nome [--prefix p]`**: renomeia namespace/assembly (sourceName `Template.Plugins`) e o
  **prefixo de schema** (centralizado em `Common.Publisher.Prefixo`). Empacotado por `Template.Pack.csproj`.
- **Só projetos novos** (não se "mergeia" em existente). Alternativa sem ferramenta: **"Use this template"**.
- **Não injeta plugins**: build gera DLL; testes rodam em memória; registro de step é **opt-in** (Plugin Registration).
- **Licença MIT** (raiz + `hipocampo/`); **disclaimer** de não-afiliação à Microsoft; nenhum binário MS redistribuído.
- **Brain por projeto**: knowledge/ADRs são semente genérica (manter); `log.md` é específico → zerar e re-capturar.

## Relacionadas
- [[escopo-e-stack]] · [[ci-cd-e-release]]
