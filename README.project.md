# Template.Plugins — D365 Customer Engagement

Projeto **Dynamics 365 Customer Engagement** (Dataverse): plugins C# (net462) + web
resources TypeScript, em **Screaming Architecture + Clean Code** (básica), com **memória
de projeto** (hipocampo). Gerado a partir do template `d365ce`.

> Projeto **privado**. Soluções D365 são proprietárias — este repo guarda **só
> código-fonte + o padrão**; solutions exportadas do D365 **não** entram (pesam demais).

## Estrutura

```
src/
├── plugins/Template.Plugins/   # assembly C# (vertical slice) — Contas/ · Contatos/ · Integracao/ · Common/
└── webresources/tpl/           # TypeScript (módulos + esbuild) → dist/ (JS que sobe)
tests/
└── Template.Plugins.Tests/     # xUnit + harness de fakes no repo (sem dependência externa)
docs/
├── architecture/               # O PADRÃO escrito (plugins, solutions, webresources, testing, alm)
└── brain/                      # memória durável (hipocampo)
```

## Build & testes

- **Solution**: `dotnet build Template.Plugins.slnx`
- **Plugins**: `dotnet build src/plugins/Template.Plugins`
- **Testes C#**: `dotnet test tests/Template.Plugins.Tests`
- **Web resources**: `cd src/webresources/tpl && npm ci && npm run build` · testes: `npm test`

## Princípios

- **1 plugin = 1 responsabilidade = 1 step** (cada plugin funciona como um método).
- **Vertical slice (Screaming)**: uma pasta/namespace por feature (`Contas/`, `Contatos/`…) reunindo
  model + repositório + service + plugins. Domínio em **PT**, infra em **EN**.
- **Sem interfaces e sem DI**: classes concretas montadas com `new`. Regra trivial no plugin; **regra no
  service, query no repositório**; entidades tipadas (`conta.Nome`, não `entity["..."]`).
- `Common`/`Integracao` não dependem de feature.

## Memória (hipocampo)

Memória versionada e revisada por humano. `/capture` propõe notas, `/search` consulta o
vault em `docs/brain/`. Núcleo é **Python puro (stdlib)**, funciona em **qualquer agente**
— **Claude Code** (hooks automáticos), **Gemini CLI** e **Codex** — ou via CLI:
`python3 -m hipocampo.search`. O **doc-sync** sugere atualizar o doc de arquitetura junto
com mudanças em `src/`, mas é **advisory** (`[enforcement] pre_commit = "warn"`): avisa, não
bloqueia o dev. A integridade do vault (links/proveniência) segue como gate no push/PR
(`pre_push`/`ci = "block"`).

> **Por onde começar**: leia `AGENTS.md` (roteador) e `docs/architecture/`. CI em
> `.github/workflows/ci.yml` roda testes (C#/TS) + gate de integridade no PR.

## Aviso

Projeto que usa **Dynamics 365**, **Dataverse** e **Power Apps** — marcas da Microsoft,
citadas de forma **descritiva**, sem afiliação nem endosso. O SDK (`Microsoft.CrmSdk.*`) é
referência **NuGet** (restaurada no build); nenhum binário da Microsoft é redistribuído.
