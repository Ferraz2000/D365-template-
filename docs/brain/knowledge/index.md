---
title: Knowledge — índice de navegação
type: index
area: meta
status: active
created: 2026-06-14
updated: 2026-06-14
tags: [knowledge, index, navigation]
---

# Knowledge — índice de navegação

> **Entry point barato** (padrão Karpathy LLM-wiki). Cada página abaixo tem 1 linha de
> what's-it-about + suas fontes. Agentes leem ESTE → carregam só as páginas relevantes.
> Nunca brute-force o vault inteiro. Mantido pelo `/capture`; `vault_sync` enforça consistência.

## meta
- [escopo-e-stack](meta/escopo-e-stack.md) — escopo do template (só código, PT-BR, team/PR, net462, D365 CE). Fontes: raw/sources/2026-06-14-sessao-decisoes.md.
- [template-reutilizavel](meta/template-reutilizavel.md) — dotnet new + prefixo centralizado + GitHub Template; MIT/público; não injeta plugins. Fontes: sessao-decisoes.
- [doc-sync-enforcement-gate](meta/doc-sync-enforcement-gate.md) — doc-sync via `[enforcement]` por ponto (pre_commit=warn advisory; pre_push/ci=block integridade). Fontes: inferido.
- [template-seed-dir-generation](meta/template-seed-dir-generation.md) — geração mapeia seed/brain→docs/brain (curado, auto-consistente); vault real intacto. Fontes: inferido.
- [publicacao-nuget-trusted-publishing](meta/publicacao-nuget-trusted-publishing.md) — publica no nuget.org via Trusted Publishing (OIDC, keyless); release.yml + policy + NUGET_USER. Fontes: inferido.

## architecture
- [vertical-slice-screaming](architecture/vertical-slice-screaming.md) — assembly por feature (namespace = feature) + regra de dependência. Fontes: sessao-decisoes, padroes-plugin.
- [dominio-pt-infra-en](architecture/dominio-pt-infra-en.md) — domínio em PT, infra em EN; logical names só nos Fields. Fontes: sessao-decisoes.
- [sem-interfaces-sem-di](architecture/sem-interfaces-sem-di.md) — classes concretas, dependências com `new`; júnior-friendly. Fontes: sessao-decisoes, padroes-plugin.
- [testes](architecture/testes.md) — harness próprio (sem FakeXrmEasy), net462 via Mono, testes de arquitetura. Fontes: sessao-decisoes.

## plugins
- [padroes-de-plugin](plugins/padroes-de-plugin.md) — 1 plugin = 1 step, early-bound, gatilhos (pre/post/preimage/custom/anti-loop). Fontes: padroes-plugin, sessao-decisoes.
- [integracoes](plugins/integracoes.md) — Service Bus (recomendado) e HTTP com retry, sempre async. Fontes: padroes-plugin, ms-learn-alm.

## webresources
- [typescript-esbuild](webresources/typescript-esbuild.md) — TS em módulos ES + esbuild (bundle IIFE) + Jest. Fontes: sessao-decisoes.

## alm
- [solutions-fora-do-repo](alm/solutions-fora-do-repo.md) — só código no repo; solution como artefato managed. Fontes: ms-learn-alm, sessao-decisoes.
- [ci-cd-e-release](alm/ci-cd-e-release.md) — CI (cache/concurrency/path-filter) + release no GitHub Packages sem key. Fontes: sessao-decisoes.
