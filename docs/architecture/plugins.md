# Arquitetura de Plugins — Screaming + Clean (simples)

> Alvo do doc-sync: mudou `src/plugins/**`? Atualize este arquivo no mesmo commit.

## Idioma
**Domínio em português, infra em inglês.** Entidades/regras/queries em PT (`Conta`, `Contato`,
`ContaRepositorio`, `ContaServico`, `CategoriaConta`); plumbing técnico mantém convenção .NET/D365
(`PluginBase`, `LocalPluginContext`, `RepositoryBase`, `Guard`). Os *logical names* (`"account"`,
`"name"`) são fixos da plataforma e ficam só nos `Fields`.

## Princípios
- **1 plugin = 1 responsabilidade = 1 step.** Herde de `PluginBase` e implemente `Execute(context)`.
- **Regra trivial fica no plugin.** Cresceu ou mexe em dados → extrai um **service**.
- **Query sempre num Repositório** (por entidade) — nunca no plugin/service.
- **Sem interfaces e sem DI**: classes concretas, dependências com `new`.

## Camadas (dentro do assembly)
```
src/plugins/<Pub>.Plugins/
├── Plugins/<Entidade>/<Acao>Plugin.cs   # herda PluginBase, implementa Execute
├── Services/ContaServico.cs             # regra de negócio (só quando precisa)
├── Repositories/                        # acesso a dados por entidade; queries aqui
│   ├── RepositoryBase.cs                #   CRUD + Associate/Disassociate (infra)
│   ├── ContaRepositorio.cs / ContatoRepositorio.cs
├── Model/                               # entidades tipadas (early-bound)
│   ├── Conta.cs / Contato.cs / ContaEnums.cs
└── Common/                              # PluginBase, LocalPluginContext, Guard, Constants
```

## Gatilhos (exemplos em `Plugins/Conta/`)
| Gatilho | Stage | Plugin | Observação |
|---|---|---|---|
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

> Testes: `docs/architecture/testing.md` (model, repository, service e plugin testados isolados).
