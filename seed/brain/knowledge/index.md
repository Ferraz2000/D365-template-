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

## architecture
- [vertical-slice-screaming](architecture/vertical-slice-screaming.md) — assembly por feature (namespace = feature) + regra de dependência. Fontes: padroes-plugin.
- [dominio-pt-infra-en](architecture/dominio-pt-infra-en.md) — domínio em PT, infra em EN; logical names só nos Fields.
- [sem-interfaces-sem-di](architecture/sem-interfaces-sem-di.md) — classes concretas, dependências com `new`; júnior-friendly. Fontes: padroes-plugin.
- [testes](architecture/testes.md) — harness próprio (sem FakeXrmEasy), net462 via Mono, testes de arquitetura.

## plugins
- [padroes-de-plugin](plugins/padroes-de-plugin.md) — 1 plugin = 1 step, early-bound, gatilhos (pre/post/preimage/custom/anti-loop). Fontes: padroes-plugin.
- [integracoes](plugins/integracoes.md) — Service Bus (recomendado) e HTTP com retry, sempre async. Fontes: padroes-plugin, ms-learn-alm.

## webresources
- [typescript-esbuild](webresources/typescript-esbuild.md) — TS em módulos ES + esbuild (bundle IIFE) + Jest.
