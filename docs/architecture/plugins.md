# Arquitetura de Plugins — Screaming + Clean (simples)

> Alvo do doc-sync: mudou `src/plugins/**`? Atualize este arquivo no mesmo commit.

## Princípios (fáceis de seguir)
- **1 plugin = 1 responsabilidade = 1 step.**
- **Herde de `PluginBase` e implemente `Execute(context)`.** O message/stage/entity é definido no
  **Plugin Registration** (e documentado no XML da classe).
- **Regra trivial fica no plugin.** Quando a regra cresce ou mexe em dados → extrai um **service**.
- **Query sempre num Repository** (por entidade) — nunca no plugin/service.
- **Sem interfaces e sem framework de DI**: classes concretas, dependências montadas com `new`.

## Camadas (dentro do assembly)
```
src/plugins/<Pub>.Plugins/
├── Plugins/<Entidade>/<Acao>Plugin.cs   # herda PluginBase, implementa Execute
├── Services/                            # regra de negócio (só quando precisa) — classe concreta
│   └── AccountService.cs
├── Repositories/                        # acesso a dados por entidade; queries aqui
│   ├── RepositoryBase.cs                #   CRUD comum sobre IOrganizationService
│   ├── AccountRepository.cs             #   ex.: GetByName (QueryExpression)
│   └── ContactRepository.cs
├── Model/                               # entidades tipadas (early-bound)
│   ├── Account.cs  [EntityLogicalName("account")]
│   └── Contact.cs
└── Common/                              # PluginBase, LocalPluginContext, Guard, Constants
```

## Dois tamanhos de plugin

**Simples — regra mora no plugin:**
```csharp
public sealed class AtualizarNomePlugin : PluginBase
{
    protected override void Execute(LocalPluginContext context)
    {
        if (!context.TryGetTarget<Model.Account>(out var account)) return;
        if (string.IsNullOrWhiteSpace(account.Name)) return;
        account.Name = account.Name.Trim();   // regra simples, aqui mesmo
    }
}
```

**Com regra + dados — extrai service (montado com `new`):**
```csharp
public sealed class AtualizarRelacionamentoPlugin : PluginBase
{
    protected override void Execute(LocalPluginContext context)
    {
        if (!context.TryGetTarget<Model.Account>(out var account)) return;
        account.Id = context.PluginContext.PrimaryEntityId;

        var service = new AccountService(new ContactRepository(context.UserService));
        service.PropagarContatoPrincipal(account);
    }
}
```

## Regra de dependência
```
Plugin → (Service, quando precisa) → Repository (por entidade) → IOrganizationService
Plugin/Service → Model (entidades tipadas)
```
- Nada de query no plugin/service: quem fala com o Dataverse é o **repositório da entidade**.
- `PluginBase` dá: `TryGetTarget<T>`, `GetPreImage<T>`/`GetPostImage<T>`, `MessageName`/`Stage`/`Depth`,
  `UserService`/`SystemService`, tracing e tratamento de erro padrão.

## Convenções
- Nome: `<Acao>Plugin.cs` em `Plugins/<Entidade>/`. Registrar **um** step por classe.
- Use **Pre/Post Images** (`GetPreImage<T>`) em vez de `Retrieve` extra.
- Falha de negócio → `InvalidPluginExecutionException` com mensagem clara.

> Testes: `docs/architecture/testing.md` (plugin, service e repository testados isolados).
