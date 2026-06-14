# AGENTS.md — roteador do projeto (D365 CE template)

Projeto **template para Dynamics 365 Customer Engagement** (Dataverse): plugins C#,
web resources TypeScript e PCF. Arquitetura **Screaming + Clean**, básica. Memória
em `docs/brain/` (hipocampo).

## Ordem de leitura (sempre)
1. O doc da área tocada em `docs/architecture/` (plugins / webresources / solutions / environments-alm).
2. `docs/brain/knowledge/index.md` → carregue **só** as páginas relevantes (index-first).
3. Este arquivo (paths + comandos).

## Estrutura (o que mora onde)
- `src/plugins/<Pub>.Plugins/` — assembly C#. **1 plugin = 1 responsabilidade = 1 step.** Camadas:
  - `Plugins/<Entidade>/<Acao>Plugin.cs` — **fino, sem regra**: registra o step e delega ao service. Gritam o domínio (Account, Opportunity, Case…).
  - `Services/` — **regra de negócio** (`IAccountService`/`AccountService`). Não conhece o pipeline.
  - `Repositories/` — **acesso a dados por entidade**; **as queries moram aqui** (`AccountRepository.GetByName`, etc.).
  - `Model/` — entidades tipadas (early-bound): `public class Account : Entity` (`account.Name`, não `entity["name"]`).
  - `Common/` — `PluginBase` (RegisteredEvents: message/stage/entity), `LocalPluginContext`, `Guard`, constantes.
- `src/webresources/<prefix>/` — TypeScript por feature → build `dist/` (JS que sobe).
- `src/pcf/` — controles PCF (source).
- `docs/architecture/` — **o padrão escrito** (fonte da verdade do design).
- `docs/brain/` — vault de conhecimento (memória durável).
- **Solutions NÃO vivem no repo** — exportadas só como artefato de build (`.gitignore` barra).

## Regra de dependência (Clean)
`Plugins → Services → Repositories → IOrganizationService`. Plugin depende de `IAccountService`;
service depende de `IContactRepository` (abstrações). **Plugin não tem regra de negócio** (só orquestra);
**regra nos Services**; **query nos Repositories** (por entidade), nunca no plugin/service.
`Services`/`Repositories`/`Common` não conhecem `Plugins`. Fluxo numa direção só.

## Convenções
- **1 plugin por ação**: `Plugins/Account/AtualizarNomePlugin.cs`. Nunca uma classe por
  tabela com `switch` de mensagem/estágio — cada ação é registrada no seu próprio step.
- Publisher/prefixo: **`Template` / `tpl`** (placeholder — renomear ao derivar um projeto real;
  o prefixo é praticamente imutável depois, escolha pensando em anos).
- Usar **Pre/Post Images** em vez de queries extras.
- Web resources: TypeScript compilado; pasta = prefixo do publisher; namespaces (não poluir escopo global).

## Build / test
- Plugins: `dotnet build src/plugins/<Pub>.Plugins` · testes: `dotnet test tests/Template.Plugins.Tests` (xUnit + harness de fakes no repo, sem dependência externa).
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
