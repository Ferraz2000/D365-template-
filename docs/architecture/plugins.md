# Arquitetura de Plugins — Screaming + Clean (camadas)

> Alvo do doc-sync: mudou `src/plugins/**`? Atualize este arquivo no mesmo commit.

## Princípios
- **1 plugin = 1 responsabilidade = 1 step.** Cada plugin registra **um** evento.
- **O plugin não tem regra de negócio.** Ele só orquestra: extrai o Target e delega ao service.
- **Regra de negócio nos Services.** **Queries nos Repositories** (por entidade) — nunca no plugin/service.

## Camadas (dentro do assembly)
```
src/plugins/<Pub>.Plugins/
├── Plugins/<Entidade>/<Acao>Plugin.cs   # FINO: registra o step + delega ao service
├── Services/                            # REGRA DE NEGÓCIO (sem boilerplate do SDK)
│   ├── IAccountService.cs / AccountService.cs
├── Repositories/                        # ACESSO A DADOS — por entidade, queries aqui
│   ├── RepositoryBase.cs                #   CRUD comum sobre IOrganizationService
│   ├── IAccountRepository.cs / AccountRepository.cs   (ex.: GetByName via QueryExpression)
│   └── IContactRepository.cs / ContactRepository.cs
├── Model/                               # entidades tipadas (early-bound)
│   ├── Account.cs  [EntityLogicalName("account")]
│   └── Contact.cs
└── Common/                              # PluginBase, LocalPluginContext, Guard, Constants
```

## Regra de dependência (Clean)
```
Plugins ──▶ Services (regra) ──▶ Repositories (queries) ──▶ IOrganizationService
   └────────▶ Model (entidades tipadas) ◀──────────────────────┘
Services/Repositories ──NÃO conhecem──▶ Plugins
```
- Plugin depende de `IAccountService`; Service depende de `IContactRepository` (abstrações).
- **Nada de query no plugin ou no service** — quem fala com o Dataverse é o repositório da entidade.

## PluginBase (rica)
`Common/PluginBase.cs` trata o pipeline:
- `RegisterEvent(stage, message, entityLogicalName, handler)` no **construtor** do plugin.
- No `Execute`, só dispara o handler cujo **message + stage + entity** casa com o step atual.
- Expõe config do registro (`UnsecureConfig`/`SecureConfig`), tracing e tratamento de erro padrão.
- `LocalPluginContext`: `TryGetTarget<T>`, `GetPreImage<T>`/`GetPostImage<T>`, `MessageName`/`Stage`/`Depth`,
  `UserService`/`SystemService` e `Resolve<T>()` (composition root — factory simples, sem DI no sandbox).

```csharp
public sealed class AtualizarNomePlugin : PluginBase
{
    public AtualizarNomePlugin()
        => RegisterEvent(Stages.PreOperation, Messages.Update, Model.Account.EntityLogicalName, OnExecute);

    private void OnExecute(LocalPluginContext context)        // só orquestra
    {
        if (!context.TryGetTarget<Model.Account>(out var account)) return;
        context.Resolve<IAccountService>().NormalizarNome(account);   // regra fica no service
    }
}
```

## Convenções
- Nome: `<Acao>Plugin.cs` em `Plugins/<Entidade>/`; registrar **um** evento por classe.
- Service por entidade/caso de uso; **uma regra por método**.
- Repository por entidade; **toda query (QueryExpression/FetchXML) mora aqui**.
- Use **Pre/Post Images** (`GetPreImage<T>`) em vez de `Retrieve` extra.
- Falha de negócio → `InvalidPluginExecutionException` com mensagem clara.

> Testes do assembly: `docs/architecture/testing.md` (service, repository e plugin testados isolados).
