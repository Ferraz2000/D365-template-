# Arquitetura de Plugins — Screaming + Clean (básica)

> Alvo do doc-sync: mudou `src/plugins/**`? Atualize este arquivo no mesmo commit.

## Princípio central
**1 plugin = 1 responsabilidade = 1 step de registro.** Cada plugin funciona como um
*método*: faz uma única coisa. **Nunca** uma classe por tabela com `switch` de mensagem/
estágio juntando pré e pós — isso acopla ações independentes.

## Estrutura do assembly
```
src/plugins/<Pub>.Plugins/
├── <Pub>.Plugins.csproj
├── Plugins/                 # 🗣️ grita o domínio: pasta por entidade, arquivo por ação
│   ├── Account/
│   │   ├── AtualizarNomePlugin.cs
│   │   ├── IntegracaoPlugin.cs
│   │   └── AtualizarRelacionamentoPlugin.cs
│   ├── Opportunity/
│   └── Case/
├── Common/                  # cross-cutting compartilhado
│   ├── PluginBase.cs        #   base IPlugin: contexto, IoC, tratamento de erro
│   ├── LocalPluginContext.cs
│   ├── ServiceFactory.cs    #   composição/IoC (resolve dependências)
│   ├── Guard.cs
│   └── Constants.cs
└── Repositories/            # acesso a dados atrás de abstração
    ├── IRepository.cs
    └── EntityRepository.cs
```

## Regra de dependência (Clean)
```
Plugins  ──depende de──▶  Common (abstrações) + Repositories (IRepository)
Repositories ──implementa──▶ acesso a dados (IOrganizationService)
Common / Repositories  ──NÃO conhecem──▶  Plugins
```
- O plugin é **fino**: lê o contexto → resolve dependências → chama a regra → fim.
- **Acesso a dados só via `IRepository`** — nunca `IOrganizationService` cru dentro de um plugin.
- Lógica de negócio mora no handler da ação, não no boilerplate de pipeline.

## Convenções
- Nome do arquivo/classe: `<Acao>Plugin.cs` dentro de `Plugins/<Entidade>/`.
- Cada plugin é registrado **individualmente** no seu step (mensagem/estágio/entidade).
- Use **Pre/Post Images** em vez de `Retrieve` extra.
- `sealed` nas classes de plugin; sem estado mutável de instância (plugins são reusados pelo runtime).
- Falhas de negócio → `InvalidPluginExecutionException` com mensagem clara.

## Registro (resumo)
Cada classe vira um *step* no Plugin Registration Tool / via solution:
mensagem (Create/Update/Delete/…), estágio (Pre-Validation/Pre-Operation/Post-Operation),
modo (sync/async), entidade e filtros de atributo. Um arquivo ↔ um step.
