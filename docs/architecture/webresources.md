# Web Resources & PCF — padrão

> Alvo do doc-sync: mudou `src/webresources/**` ou `src/pcf/**`? Atualize este arquivo no mesmo commit.

## Web resources (TypeScript)
- Escritos em **TypeScript**, compilados para **JavaScript** (o JS é o que sobe para o Dataverse).
- Pasta raiz nomeada com o **prefixo do publisher** (`tpl`), espelhando o nome dos web resources no D365.
- Organização **por feature**; código comum em `common/`.
- Namespaces/módulos para **não poluir o escopo global** (`Xrm` é global).

```
src/webresources/tpl/
├── package.json
├── tsconfig.json            # target ES6
├── src/
│   ├── common/              # helpers compartilhados (ex.: acesso ao formContext)
│   ├── account/             # scripts de formulário por entidade/feature
│   └── opportunity/
└── dist/                    # JS compilado (gitignored; é o que sobe)
```

## Convenções
- 1 arquivo TS por formulário/feature; funções de evento exportadas e referenciadas no form.
- Tipos do cliente (`@types/xrm`) para o `Xrm`/`formContext`.
- Build: `npm ci && npm run build` → gera `dist/` (não versionado).
- Nome do web resource no D365: `tpl_/<feature>/<arquivo>.js`.

## PCF (`src/pcf/`)
- Controles de código (Power Apps Component Framework) em TypeScript.
- Criados/inicializados com `pac pcf init`; build com `npm run build`.
- Empacotados na solution no release (artefato de build, fora do git).
