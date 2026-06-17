---
title: Publicação do template — nuget.org via Trusted Publishing (OIDC, keyless)
type: knowledge
area: meta
status: active
confidence: high
provenance: inferred
sources:
created: 2026-06-17
updated: 2026-06-17
valid_until: ""
superseded_by: ""
tags: [knowledge, meta, ci, release, nuget, dotnet-new]
---

# Publicação do template — nuget.org via Trusted Publishing

> **Verdade no brain.** O template é distribuído como pacote `dotnet new` público no
> **nuget.org**, publicado por `.github/workflows/release.yml` **sem API key longeva** —
> via **Trusted Publishing (OIDC)**. GitHub Packages recebe em paralelo (GITHUB_TOKEN).

## Contexto

nuget.org passou a desencorajar API keys longevas. O release usa Trusted Publishing:
o workflow troca o token OIDC do GitHub por uma key temporária (~1h) via
`NuGet/login@v1` — nada de secret de key pra rotacionar/vazar.

## A decisão / como funciona

`release.yml` (job `publish`) dispara em: Release published, push de tag `v*`/`V*`, ou
`workflow_dispatch` (input `version`). Versão = `inputs.version | release.tag_name |
ref_name` (sem o `v`). Passos: pack `Template.Pack.csproj` → push GitHub Packages
(`GITHUB_TOKEN`, `--skip-duplicate`) → `NuGet/login@v1` (OIDC) → push nuget.org com a
key temporária. `permissions: id-token: write` é obrigatório pro OIDC.

**Setup (uma vez):**
- nuget.org → **Trusted Publishing** → policy: Owner `Ferraz2000`, Repo `D365-template-`,
  Workflow File `release.yml` (só o nome), Environment vazio. (Repo público ativa direto;
  privado fica "ativa 7 dias" até o 1º publish, aí fixa nos IDs.)
- Secret `NUGET_USER` = username do perfil nuget.org (público; o passo nuget.org é
  pulado se ela faltar — GitHub Packages segue funcionando).

**Publicar nova versão:** `gh workflow run release.yml -f version=X.Y.Z` (ou Release/tag).
**Versão é single-sourced na tag/input** — `Template.Pack.csproj:PackageVersion` fica como
fallback, não precisa bumpar à mão. **Por quê:** keyless é mais seguro e simples; sem
rotação de secret. **Trade-off:** depende do rollout do Trusted Publishing na conta.

Pacote leva README na raiz (`PackageReadmeFile`) pra listagem. Instalação pública:
`dotnet new install D365CE.VerticalSlice.Template`.

## Relacionadas

- [[template-seed-dir-generation]]
- [[ci-cd-e-release]]
