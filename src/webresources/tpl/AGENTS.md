# AGENTS.md — web resources (`tpl`)

Roteador local. Leia antes o roteador raiz `AGENTS.md` e `docs/architecture/webresources.md`.

## O que mora aqui
- `src/*.ts` — **lógica pura** (ex.: `format.ts`, `validacao.ts`), sem `Xrm`, testada com Jest.
- `src/account/form.ts` — entry-point de formulário **fino** que delega à lógica pura.
- `*.test.ts` — testes Jest co-localizados.

## Regras / comandos
- TypeScript em **módulos ES**; build com **esbuild** → bundle IIFE (`--global-name=Tpl`); o JS de `dist/` é o que sobe.
- Lógica de negócio em módulo puro; entry-points finos. Registro no D365: `Tpl.onLoad`.
- `npm ci && npm test` (Jest) · `npm run build` (esbuild) · `npm run typecheck` (tsc).
- Mudou `src/webresources/**`? Atualize `docs/architecture/webresources.md` no mesmo commit (doc-sync).

## Conhecimento durável
`docs/brain/knowledge/webresources/typescript-esbuild.md`.
