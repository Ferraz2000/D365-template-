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
- Cobertos: model (`Conta`), `ContaRepositorio` (queries), `ContaServico`, e os plugins (pré/pós/PreImage/custom message/anti-loop).

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
| **C# (net462, via Mono)** | ✅ 20 testes, 0 falhas |
| **TypeScript (Jest)** | ✅ 5 testes, 2 suítes |

> Toolchain instalada no container: **.NET SDK 8** (build de net462 com reference assemblies)
> e **Mono** (executa net462 em Linux). Em dev Windows / CI, use `dotnet test` direto.
> Observação: o **.NET Framework 4.8 é só Windows**; em Linux quem roda net462/4.x é o Mono.

## Princípio
Cada unidade testável = uma responsabilidade. Plugin fino + regra isolada + acesso a dados
atrás de `IRepository` torna tudo **mockável** — é o que destrava ter testes de verdade.
