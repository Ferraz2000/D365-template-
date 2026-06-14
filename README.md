# Template D365 Customer Engagement

Template de projeto para **Dynamics 365 Customer Engagement** (Dataverse): plugins C#,
web resources TypeScript — com **Screaming Architecture + Clean Code** (básica) e
**memória de projeto** (hipocampo).

## Princípios
- **1 plugin = 1 responsabilidade = 1 step** (cada plugin funciona como um método).
- **Vertical slice (Screaming)**: uma pasta/namespace por feature (`Contas/`, `Contatos/`…) reunindo
  model + repositório + service + plugins. Domínio em **PT**, infra em **EN**.
- **Sem interfaces e sem DI**: classes concretas montadas com `new`. Regra trivial no plugin; **regra no
  service, query no repositório**; entidades tipadas (`conta.Nome`, não `entity["..."]`).
- Repo guarda **só código-fonte + o padrão**. Solutions exportadas do D365 **não** entram (pesam demais).

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

## Reutilizar como template
**Projeto novo?** `dotnet new install .` → `dotnet new d365ce -n MeuProjeto --prefix ctso`. Ver `TEMPLATE.md`.
O template **não injeta nada na sua org** — exemplos vêm prontos e inertes; registro de step é opt-in.

## Por onde começar
0. **Dev novo?** Comece pelo `CONTRIBUTING.md` (build, test, como adicionar um plugin/feature).
1. Leia `AGENTS.md` (roteador) e `docs/architecture/`.
2. Renomeie o placeholder **`Template`/`tpl`** para o publisher/prefixo real (o prefixo é praticamente imutável).
3. Plugins: `dotnet build src/plugins/Template.Plugins` · testes: `dotnet test tests/Template.Plugins.Tests`.
4. Web resources: `cd src/webresources/tpl && npm ci && npm run build` · testes: `npm test`.

## Convenções de plugin (resumo)
- `Contas/<Acao>Plugin.cs` — herda `PluginBase`, implementa `Execute`. **1 plugin = 1 step.**
- **Regra trivial no plugin**; quando cresce/mexe em dados → um **service** na mesma feature. **Queries** sempre no **repositório da entidade**.
- Use **entidades tipadas**: `context.TryGetTarget<Conta>(out var conta)` → `conta.Nome`. **Domínio em PT, infra em EN.**
- **Sem interfaces e sem DI**: classes concretas com `new` (ex.: `new ContaServico(new ContaRepositorio(ctx.UserService), new ContatoRepositorio(ctx.UserService))`).
- Cada feature numa pasta/namespace (`Contas/`, `Contatos/`…); `Common`/`Integracao` não dependem de feature.

## Memória (hipocampo)
Sistema de memória versionada e revisada por humano. `/capture` propõe notas, `/search`
consulta o vault em `docs/brain/`. O **doc-sync gate** (pre-commit) exige que mudanças em
`src/` venham com o doc de arquitetura atualizado no mesmo commit.

> **CI**: `.github/workflows/ci.yml` roda testes (C#/TS) + doc-sync no PR. ALM/CD de solution:
> ver `docs/architecture/environments-alm.md`.

## Licença
**MIT** — use à vontade (ver `LICENSE`). Inclui o `hipocampo/` vendorizado (também MIT, mesmo autor).

## Aviso
Projeto **da comunidade**, **não afiliado nem endossado pela Microsoft**. *Dynamics 365*,
*Dataverse* e *Power Apps* são marcas da Microsoft, usadas aqui de forma **descritiva**. O SDK
(`Microsoft.CrmSdk.*`) é referência **NuGet** (restaurada no build) — nenhum binário da Microsoft é redistribuído neste repo.
