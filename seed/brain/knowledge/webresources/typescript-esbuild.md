---
title: Web resources em TypeScript (módulos + esbuild + Jest)
type: knowledge
area: webresources
status: active
confidence: high
provenance: inferred
sources:
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [webresources, typescript]
---

# Web resources em TypeScript

> Web resources são **TypeScript** em **módulos ES**, empacotados com **esbuild** (bundle IIFE,
> `--global-name=Tpl`). O JS de `dist/` é o que sobe. Lógica pura testada com **Jest**.

## A decisão / conceito
- `src/*.ts` (lógica pura, ex.: `validacao`, `format`) → testável sem `Xrm`.
- Entry-points de formulário finos delegam à lógica pura. Registro no D365: `Tpl.onLoad`.

## Relacionadas
- [[escopo-e-stack]]
