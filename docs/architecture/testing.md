# Testes — padrão

> Muitos projetos D365 não têm testes. Este template já nasce com a estrutura pronta — e
> as suítes **rodam de verdade**, sem depender de ambiente Windows nem de licença paga.

## Plugins (C#) — xUnit + harness de fakes no repo
Projeto: `tests/Template.Plugins.Tests/` (TFM **net462**, igual ao assembly).

- **Sem dependência externa de mock.** O harness em `Fakes/FakeHarness.cs` simula o pipeline
  (`IServiceProvider`, `IPluginExecutionContext`) e um `IOrganizationService` em memória.
  Roda em **.NET Framework, Mono e CI** — não usa FakeXrmEasy (evita a questão de licença v2/v3).
- Padrão: Arrange (montar Target tipado + contexto) → Act (`harness.Execute<TPlugin>`) → Assert.
- Cada camada testa isolada: **Plugins** (via `harness`), **Services** (regra, sem pipeline),
  **Repositories** (queries via fake `IOrganizationService`). Classes concretas → testa com `new`.
- Cobertos: model (`Conta`), `ContaRepositorio` (queries), `ContaServico`, plugins (pré/pós/PreImage/custom message/anti-loop) e integrações.
- **Integrações**: `ClienteRest` testado com `HttpMessageHandler` falso (sem rede); Service Bus com `IServiceEndpointNotificationService` falso.
- **Testes de arquitetura** (`Arquitetura/`, vertical slice): `Common`/`Integracao` não dependem
  de feature; `Contatos` não depende de `Contas`; plugins são `sealed`/herdam `PluginBase`/terminam em `Plugin` e
  **não são referenciados** (pontos de entrada); repositórios herdam `RepositoryBase`.
  - `ArquiteturaTests.cs` — via **reflection** (nível de API), roda em qualquer lugar.
  - `ArquiteturaNetArchTests.cs` — via **NetArchTest/IL** (pega corpo de método); **pula no Mono**, roda no **CI Windows**.

## CI (GitHub Actions — `.github/workflows/ci.yml`)
No PR: **doc-sync** (preflight, ubuntu/python), **testes C#** (windows, `dotnet test` net462 nativo),
**testes TS** (ubuntu, web resources). É o espelho do gate local, mas que ninguém pula com `--no-verify`.

```sh
dotnet test tests/Template.Plugins.Tests        # Windows / CI
```

> **FakeXrmEasy (opcional):** para cenários de query/FetchXML mais ricos, dá para adicionar
> `FakeXrmEasy` ao projeto. A linha **1.x** é MIT/grátis; **v2/v3** exigem licença. O template
> não depende dele de propósito, para a suíte rodar em qualquer lugar.

## Web resources (TypeScript) — Jest + ts-jest
Co-localizados em `src/webresources/tpl/` (arquivos `*.test.ts`).

- Lógica em **módulos puros** (`src/format.ts`) → testável sem mockar o `Xrm`.
- Entry-points de formulário finos, testados com um `formContext` mínimo mockado.

```sh
cd src/webresources/tpl
npm ci && npm test && npm run build
```

## Verificado neste scaffolding
| Suíte | Resultado |
|---|---|
| **C# (net462, via Mono)** | ✅ 43 testes, 0 falhas (3 de arquitetura IL pulam no Mono → rodam no CI Windows) |
| **TypeScript (Jest)** | ✅ 12 testes (web resources) |

> Toolchain instalada no container: **.NET SDK 8** (build de net462 com reference assemblies)
> e **Mono** (executa net462 em Linux). Em dev Windows / CI, use `dotnet test` direto.
> Observação: o **.NET Framework 4.8 é só Windows**; em Linux quem roda net462/4.x é o Mono.

## Princípio
Cada unidade testável = uma responsabilidade. Plugin fino + regra no service + acesso a dados
no repositório (sobre `IOrganizationService`) torna tudo **mockável** — é o que destrava testes de verdade.
