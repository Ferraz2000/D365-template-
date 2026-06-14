# Template D365 Customer Engagement

Template de projeto para **Dynamics 365 Customer Engagement** (Dataverse): plugins C#,
web resources TypeScript e PCF — com **Screaming Architecture + Clean Code** (básica) e
**memória de projeto** (hipocampo).

## Princípios
- **1 plugin = 1 responsabilidade = 1 step** (cada plugin funciona como um método).
- Três blocos no assembly: **`Plugins`** (grita o domínio) · **`Common`** · **`Repositories`**.
- Regra de dependência Clean: plugins dependem de **abstrações** (`IRepository`), nunca de `IOrganizationService` cru.
- Repo guarda **só código-fonte + o padrão**. Solutions exportadas do D365 **não** entram (pesam demais).

## Estrutura
```
src/
├── plugins/Template.Plugins/   # assembly C# — Plugins/ · Services/ · Repositories/ · Model/ · Common/
├── webresources/tpl/           # TypeScript (módulos + esbuild) → dist/ (JS que sobe)
└── pcf/                         # controles PCF
tests/
└── Template.Plugins.Tests/     # xUnit + harness de fakes no repo (sem dependência externa)
docs/
├── architecture/               # O PADRÃO escrito (plugins, solutions, webresources, testing, alm)
└── brain/                      # memória durável (hipocampo)
```

## Por onde começar
1. Leia `AGENTS.md` (roteador) e `docs/architecture/`.
2. Renomeie o placeholder **`Template`/`tpl`** para o publisher/prefixo real (o prefixo é praticamente imutável).
3. Plugins: `dotnet build src/plugins/Template.Plugins` · testes: `dotnet test tests/Template.Plugins.Tests`.
4. Web resources: `cd src/webresources/tpl && npm ci && npm run build` · testes: `npm test`.

## Convenções de plugin (resumo)
- `Plugins/<Entidade>/<Acao>Plugin.cs` — **fino**: `RegisterEvent(...)` no construtor + delega ao service. **Sem regra de negócio.**
- **Regra de negócio** em `Services/`; **queries** em `Repositories/` (por entidade).
- Use **entidades tipadas** (`Model/`): `context.TryGetTarget<Account>(out var account)` → `account.Name`.
- `PluginBase` casa o step por **message + stage + entity**; 1 evento por classe.

## Memória (hipocampo)
Sistema de memória versionada e revisada por humano. `/capture` propõe notas, `/search`
consulta o vault em `docs/brain/`. O **doc-sync gate** (pre-commit) exige que mudanças em
`src/` venham com o doc de arquitetura atualizado no mesmo commit.

> ALM/CI-CD: documentado em `docs/architecture/environments-alm.md` como próximo passo
> (fora do escopo deste skeleton).
