---
title: CI/CD do template: eficiente e sem key
type: knowledge
area: alm
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-sessao-decisoes.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [alm, ci, release]
---

# CI/CD do template

> Dois workflows: **`ci.yml`** (PR/push) e **`release.yml`** (publicação). Eficientes e sem segredo do usuário.

## A decisão / conceito
- **CI**: jobs paralelos — `doc-sync` (sempre), `testes-csharp` (Windows, net462 nativo), `testes-typescript`.
  Com **cache** (NuGet + npm), **concurrency** (cancel-in-progress) e **path filters** (não roda o Windows à toa).
- **Release**: ao publicar um Release (tag `v*`), packa o `dotnet new` template e publica no **GitHub Packages**
  via `GITHUB_TOKEN` (**sem API key**); passo opcional pro nuget.org se houver a secret `NUGET_API_KEY`.
- **Por quê Windows pro C#**: net462 roda nativo lá (sem Mono); é o job mais caro, por isso os path filters.
- Os workflows são **vendorizados** → valem automaticamente nos projetos gerados (paths renomeados pelo `dotnet new`).

## Relacionadas
- [[testes]] · [[template-reutilizavel]]
