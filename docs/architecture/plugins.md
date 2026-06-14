# Arquitetura de Plugins — Screaming + Clean (simples)

> Alvo do doc-sync: mudou `src/plugins/**`? Atualize este arquivo no mesmo commit.

## Idioma
**Domínio em português, infra em inglês.** Entidades/regras/queries em PT (`Conta`, `Contato`,
`ContaRepositorio`, `ContaServico`, `CategoriaConta`); plumbing técnico mantém convenção .NET/D365
(`PluginBase`, `LocalPluginContext`, `RepositoryBase`, `Guard`). Os *logical names* (`"account"`,
`"name"`) são fixos da plataforma e ficam só nos `Fields`.

## Prefixo & registro (opt-in)
- **Prefixo de schema custom** centralizado em `Common.Publisher.Prefixo` (default `tpl`): colunas
  custom (`tpl_servicos`, `tpl_resumo`) e Custom API (`tpl_CalcularScoreConta`) derivam dele. Troque
  em um só ponto (ou via `dotnet new ... --prefix ctso` — ver `TEMPLATE.md`). Campos padrão não usam prefixo.
- **O template não injeta plugins na org.** Build gera só um DLL; o plugin entra em vigor quando **você
  registra o step** (Plugin Registration / `pac`), opt-in por plugin. Ver gatilhos abaixo e `TEMPLATE.md`.

## Princípios
- **1 plugin = 1 responsabilidade = 1 step.** Herde de `PluginBase` e implemente `Execute(context)`.
- **Regra trivial fica no plugin.** Cresceu ou mexe em dados → extrai um **service**.
- **Query sempre num Repositório** (por entidade) — nunca no plugin/service.
- **Sem interfaces e sem DI**: classes concretas, dependências com `new`.

## Estrutura — vertical slice (Screaming)
O topo do assembly **grita o domínio**: cada feature é uma pasta/namespace com tudo dela.
```
src/plugins/<Pub>.Plugins/
├── Contas/                              # 🗣️ feature Conta (namespace Template.Plugins.Contas)
│   ├── Conta.cs / ContaEnums.cs         #   model + enums (early-bound)
│   ├── ContaRepositorio.cs              #   queries da conta
│   ├── ContaServico.cs                  #   regra de negócio
│   ├── ContaPayload.cs                  #   payload de integração (puro)
│   └── *Plugin.cs                       #   ações (8 plugins)
├── Contatos/                            # feature Contato
│   ├── Contato.cs / ContatoRepositorio.cs
├── Integracao/ClienteRest.cs           # infra compartilhada (HTTP)
└── Common/                              # PluginBase, LocalPluginContext, RepositoryBase, Guard, Constants
```
Regra entre features: dependência **numa direção** (`Contas → Contatos`); `Common`/`Integracao`
não dependem de feature nenhuma. As camadas (model/repo/service/plugin) viram **arquivos** dentro
da feature — a separação é por convenção (e pelos testes de arquitetura).

Cada feature tem um **`AGENTS.md` local** (`Contas/AGENTS.md`, `Contatos/AGENTS.md`) com as regras
da feature, além do roteador raiz e deste doc.

## Integrações
| Padrão | Plugin | Como |
|---|---|---|
| **Service Bus (recomendado, desacoplado)** | `PublicarEventoContaPlugin` | `context.PostarNaFila(serviceEndpointId)` → posta o contexto na fila via `IServiceEndpointNotificationService` |
| **HTTP REST (async)** | `IntegracaoPlugin` | `new ClienteRest(httpClient).PostJson(url, json)` — com **retry** |

- **Nunca** chamar serviço externo em step **síncrono** (prende a transação) — use **async**.
- `ClienteRest` recebe o `HttpClient` por construtor → testável com handler falso, sem rede; faz **retry** em falha transitória.
- Preferir a fila (Service Bus) ao HTTP direto: resiliente a falhas e não acopla o CRM ao externo.

## Concorrência otimista
`ContaRepositorio.AtualizarComConcorrencia(conta, rowVersion)` usa `UpdateRequest` com
`ConcurrencyBehavior.IfRowVersionMatches` — só grava se a `RowVersion` não mudou (senão erro de
concorrência). Evita sobrescrever a alteração de outro usuário.

## Gatilhos (exemplos em `Contas/`)
| Gatilho | Stage | Plugin | Observação |
|---|---|---|---|
| **Pre-Validation** | 10 | `ValidarContaPlugin` | valida cedo, **fora** da transação; lança `InvalidPluginExecutionException` |
| **Pre-Operation** | 20 | `AtualizarNomePlugin`, `ClassificarContaPlugin` | alterar o Target já basta |
| **Post-Operation** | 40 | `AtualizarRelacionamentoPlugin`, `IntegracaoPlugin` | já gravado; use repositório |
| **Post + PreImage** | 40 | `RegistrarMudancaNomePlugin` | `GetPreImage<T>("preimage")` traz o valor anterior |
| **Custom Message** | 30 | `CalcularScoreContaPlugin` | Custom API `tpl_CalcularScoreConta`; `GetInput`/`SetOutput` |
| **Anti-loop (Depth)** | 40 | `RecalcularResumoContaPlugin` | atualiza a própria conta; `if (context.Depth > 1) return;` |

## Tipos do D365 no Model (`Model/Conta.cs`)
| Tipo D365 | No código | Exemplo |
|---|---|---|
| Texto | `string` | `Nome` |
| Lookup (N:1) | `EntityReference` | `ContatoPrincipalId` |
| Money | `decimal?` | `Receita` |
| OptionSet | `enum?` | `Categoria` → `CategoriaConta` |
| **Multi-select OptionSet** | `enum[]` (`OptionSetValueCollection`) | `Servicos` → `Servico[]` |
| State | `enum?` | `Estado` → `EstadoConta` |
| Inteiro / Data | `int?` / `DateTime?` | `NumeroDeFuncionarios` / `UltimaEspera` |

## Queries (`Repositories/ContaRepositorio.cs`)
- **Igualdade** → `ObterPorNome`
- **Like + ordenação** → `BuscarPorNome`
- **OptionSet + In** → `ListarPorCategoria`
- **Money + filtro/ordem/top** → `TopPorReceita`
- **Filtro composto AND/OR** → `ListarAtivasPreferenciaisOuGrandes`
- **Multi-select (ContainValues)** → `ComServico`
- **Relacionamento N:1 (link-entity + AliasedValue)** → `NomesDosContatosPrincipais`
- **N:N (Associate)** → `AssociarContatos`

> Regra que **consome** query: `ContaServico.RejeitarSeNomeDuplicado` usa `ObterPorNome`.

## Convenções
- Nome: `<Acao>Plugin.cs` em `Plugins/<Entidade>/`. Registrar **um** step por classe.
- Use **Pre/Post Images** (`GetPreImage<T>`) em vez de `Retrieve` extra.
- **Anti-loop**: ao atualizar a própria entidade, guardar com `context.Depth > 1`.
- Falha de negócio → `InvalidPluginExecutionException` com mensagem clara.

## Faltou (sugestões — peça que eu adiciono)
- **DateTime/timezone** (UTC ↔ local), **lookup polimórfico** (Customer = account|contact),
  **FetchXML** (alternativa a QueryExpression), **paginação** (`PagingInfo`/cookie),
  **ExecuteMultiple** (lote), **Upsert**, **rollup/calculated** (read-only),
  **shared variables** entre steps, **Activities** (task/email).
- **Precisa de infra externa** (não dá para cobrir no harness local): teste **E2E em sandbox real**
  e arquitetura no **nível de corpo de método** (NetArchTest/Mono.Cecil em CI). O gate de doc-sync
  é auto-verificável com `python -m hipocampo.canary`.

> Testes: `docs/architecture/testing.md` (model, repository, service e plugin testados isolados).
