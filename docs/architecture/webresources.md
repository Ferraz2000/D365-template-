# Web Resources & PCF — padrão

> Alvo do doc-sync: mudou `src/webresources/**` ou `src/pcf/**`? Atualize este arquivo no mesmo commit.

## Web resources (TypeScript, módulos ES + bundle)
- Escritos em **TypeScript** como **módulos ES**, empacotados com **esbuild** em um bundle
  **IIFE** (`--global-name=Tpl`); o **JS de `dist/`** é o que sobe para o Dataverse.
- Pasta raiz nomeada com o **prefixo do publisher** (`tpl`).
- **Lógica em módulos puros** (testáveis), **entry-points de formulário finos** que delegam.
- Organização por feature; comum em módulos reutilizáveis.

```
src/webresources/tpl/
├── package.json            # scripts: build (esbuild), test (jest), typecheck (tsc)
├── tsconfig.json           # module ESNext, target ES2017, types: xrm + jest
├── jest.config.js          # ts-jest
├── src/
│   ├── format.ts           # lógica pura (sem Xrm) — testável
│   ├── format.test.ts
│   └── account/
│       ├── form.ts         # OnLoad fino → delega a format
│       └── form.test.ts    # mocka formContext mínimo
└── dist/                   # bundle IIFE (gitignored; é o que sobe)
```

> Roteador local: `src/webresources/tpl/AGENTS.md` resume as regras desta pasta.

## Validações (módulos puros)
Regras de validação em **módulos puros** (`src/validacao.ts`: `emailValido`, `cnpjValido`) — sem `Xrm`,
fáceis de testar com Jest. Os entry-points de formulário chamam essas funções.

## Ferramental
- **Build com source map** (`esbuild --sourcemap`) → debug no F12 do navegador.
- **Testes**: lógica pura sem mock; entry-points de formulário com **`xrm-mock`** (mock padrão do `Xrm`).
- **Lint/format**: ESLint (flat config + typescript-eslint) e Prettier — `npm run lint` / `npm run format`.

## Convenções
- 1 entry-point por formulário/feature; funções de evento exportadas.
- Regra de negócio em módulo puro (sem `Xrm`) → testar sem mock pesado.
- Registro no D365: OnLoad → `Tpl.onLoad` (nome do global do bundle).
- Build: `npm ci && npm run build` → `dist/` (não versionado). Testes: `npm test` (ver `testing.md`).
- Nome do web resource no D365: `tpl_/<feature>/<arquivo>.js`.

## PCF (`src/pcf/`)
- Controles de código (Power Apps Component Framework) em TypeScript.
- Criados com `pac pcf init`; build com `npm run build`.
- **Lógica pura testável** em `src/*.ts` (ex.: `logic.ts` → `rotuloEstrelas`), com Jest (`npm test`) —
  o controle real (`init`/`updateView`) delega a essas funções.
- Empacotados na solution no release (artefato de build, fora do git).
