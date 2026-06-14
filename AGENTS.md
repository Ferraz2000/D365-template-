# AGENTS.md — roteador do projeto (D365 CE template)

Projeto **template para Dynamics 365 Customer Engagement** (Dataverse): plugins C#,
web resources TypeScript e PCF. Arquitetura **Screaming + Clean**, básica. Memória
em `docs/brain/` (hipocampo).

## Ordem de leitura (sempre)
1. O doc da área tocada em `docs/architecture/` (plugins / webresources / solutions / environments-alm).
2. `docs/brain/knowledge/index.md` → carregue **só** as páginas relevantes (index-first).
3. Este arquivo (paths + comandos).

## Estrutura (o que mora onde)
- `src/plugins/<Pub>.Plugins/` — assembly C#. **1 plugin = 1 responsabilidade = 1 step.**
  - `Plugins/<Entidade>/<Acao>Plugin.cs` — handlers finos, **gritam o domínio** (Account, Opportunity, Case…).
  - `Common/` — `PluginBase`, `LocalPluginContext`, IoC, `Guard`, constantes (cross-cutting).
  - `Repositories/` — `IRepository` + implementações (acesso a dados atrás de abstração).
- `src/webresources/<prefix>/` — TypeScript por feature → build `dist/` (JS que sobe).
- `src/pcf/` — controles PCF (source).
- `docs/architecture/` — **o padrão escrito** (fonte da verdade do design).
- `docs/brain/` — vault de conhecimento (memória durável).
- **Solutions NÃO vivem no repo** — exportadas só como artefato de build (`.gitignore` barra).

## Regra de dependência (Clean)
`Plugins/` → depende de **abstrações** (`Common`, `IRepository`). Nunca `IOrganizationService`
cru dentro de um plugin: acesso a dados só via `IRepository`. `Repositories` e `Common`
não conhecem `Plugins`. Fluxo numa direção só. Plugin é **fino**: parse do contexto →
chama a regra → fim. Lógica de negócio fora do `Execute` boilerplate.

## Convenções
- **1 plugin por ação**: `Plugins/Account/AtualizarNomePlugin.cs`. Nunca uma classe por
  tabela com `switch` de mensagem/estágio — cada ação é registrada no seu próprio step.
- Publisher/prefixo: **`Template` / `tpl`** (placeholder — renomear ao derivar um projeto real;
  o prefixo é praticamente imutável depois, escolha pensando em anos).
- Usar **Pre/Post Images** em vez de queries extras.
- Web resources: TypeScript compilado; pasta = prefixo do publisher; namespaces (não poluir escopo global).

## Build / test
- Plugins: `dotnet build src/plugins/<Pub>.Plugins` · testes: `dotnet test tests/Template.Plugins.Tests` (xUnit + FakeXrmEasy).
- Web resources: em `src/webresources/<prefix>/` → `npm ci && npm run build` · testes: `npm test` (Jest).
- Padrão de testes: `docs/architecture/testing.md`. **Todo plugin/regra nova vem com teste.**
- Empacotar/deploy: `pac solution ...` — ver `docs/architecture/environments-alm.md` (artefato de build, fora do git).

## Fluxo de trabalho (team)
- Trabalhe em branch de feature → **PR** para `main`. Sem commit direto em `main`.
- Sugestão: **proteger** `main` e a pasta do vault (`docs/brain/`) com review obrigatório.
- **Doc-sync gate** (pre-commit): mudou `src/plugins/**`, `src/webresources/**` ou `src/pcf/**`?
  Atualize o doc correspondente em `docs/architecture/` **no mesmo commit**. Não use
  `git commit --no-verify` sem autorização explícita.

## Memória (hipocampo)
- `/capture <algo>` — propõe nota para curadoria humana antes de virar conhecimento.
- `/search <termos>` — consulta o vault. Decisões/contratos/lições viram conhecimento durável.
