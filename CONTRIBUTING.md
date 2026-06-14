# Contribuindo — guia do dev (júnior-friendly)

Este template é **D365 Customer Engagement**: plugins C# (net462), web resources TypeScript e PCF.
Arquitetura **vertical slice (Screaming)**, domínio em PT, infra em EN, **sem interfaces e sem DI**.

## Pré-requisitos
- **.NET SDK 8** (compila o assembly net462 com reference assemblies).
- **Node 20** (web resources / PCF).
- **Power Platform CLI (`pac`)** para registrar/empacotar (deploy).
- Windows para rodar a suíte C# com `dotnet test` (em Linux, via Mono — ver `docs/architecture/testing.md`).

## Build & test
```sh
# Plugins (C#)
dotnet build src/plugins/Template.Plugins
dotnet test  tests/Template.Plugins.Tests        # 43 testes (inclui arquitetura)

# Web resources (TypeScript)
cd src/webresources/tpl && npm ci && npm test && npm run build

# PCF
cd src/pcf && npm ci && npm test
```

## Como adicionar um plugin novo (passo a passo)
Exemplo: validar o telefone da conta antes de gravar.
1. Crie `src/plugins/Template.Plugins/Contas/ValidarTelefoneContaPlugin.cs`:
   ```csharp
   using Microsoft.Xrm.Sdk;
   using Template.Plugins.Common;

   namespace Template.Plugins.Contas
   {
       /// Registrar: Update / Pre-Validation / account / filtro=telephone1
       public sealed class ValidarTelefoneContaPlugin : PluginBase
       {
           protected override void Execute(LocalPluginContext context)
           {
               if (!context.TryGetTarget<Conta>(out var conta)) return;
               // sua regra aqui (uma só coisa)
           }
       }
   }
   ```
2. **Regra trivial fica no plugin**; se crescer ou mexer em dados, extraia para `ContaServico` e
   ponha as **queries** no `ContaRepositorio`.
3. Use **entidades tipadas** (`conta.Nome`), nunca `entity["..."]`.
4. **Escreva o teste** em `tests/.../Plugins/Conta/` usando o `PluginHarness`.
5. **Atualize `docs/architecture/plugins.md`** no mesmo commit (o doc-sync gate exige).
6. Registre o step no Plugin Registration (mensagem/estágio/entidade do XML doc).

## Como adicionar uma feature nova (ex.: `Pedidos`)
1. Crie a pasta `src/plugins/Template.Plugins/Pedidos/` (namespace `Template.Plugins.Pedidos`).
2. Coloque ali: `Pedido.cs` (entidade tipada), `PedidoRepositorio.cs`, `PedidoServico.cs` (se precisar)
   e os `*Plugin.cs`.
3. **Dependência numa direção** — uma feature pode depender de outra, nunca o contrário.
   `Common`/`Integracao` não dependem de feature. Os testes de arquitetura garantem isso.
4. Crie um `AGENTS.md` local na pasta da feature.

## Regras de ouro
- **1 plugin = 1 responsabilidade = 1 step.**
- **Regra no service, query no repositório**; plugin fino.
- Integração externa **só em step assíncrono** (prefira Service Bus ao HTTP direto).
- Anti-loop: ao atualizar a própria entidade, guarde com `context.Depth > 1`.
- Falha de negócio → `InvalidPluginExecutionException` com mensagem clara.

## Fluxo de PR
- Branch de feature → **PR** para `main` (sem commit direto em `main`).
- A **CI** roda `dotnet test` + `npm test` + doc-sync/vault-sync no PR.
- O **pre-commit** roda o doc-sync local; **não** use `git commit --no-verify` sem autorização.

## Memória do projeto
Decisões/lições viram conhecimento durável via **`/capture`** (curadoria humana) em `docs/brain/`.
Leia `docs/brain/context-budget.md` antes de consultar o vault.
