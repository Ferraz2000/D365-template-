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
├── plugins/Template.Plugins/   # assembly C# — Plugins/ · Common/ · Repositories/
├── webresources/tpl/           # TypeScript (módulos + esbuild) → dist/ (JS que sobe)
└── pcf/                         # controles PCF
tests/
└── Template.Plugins.Tests/     # xUnit + FakeXrmEasy (testes de plugin)
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
- `src/plugins/Template.Plugins/Plugins/<Entidade>/<Acao>Plugin.cs` — um arquivo por ação.
- Herde de `PluginBase`; faça **uma** coisa no `Execute`.
- Registre cada classe no seu próprio step (mensagem/estágio/entidade).

## Memória (hipocampo)
Sistema de memória versionada e revisada por humano. `/capture` propõe notas, `/search`
consulta o vault em `docs/brain/`. O **doc-sync gate** (pre-commit) exige que mudanças em
`src/` venham com o doc de arquitetura atualizado no mesmo commit.

> ALM/CI-CD: documentado em `docs/architecture/environments-alm.md` como próximo passo
> (fora do escopo deste skeleton).
