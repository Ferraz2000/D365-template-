# Testes — padrão

> Muitos projetos D365 não têm testes. Este template já nasce com a estrutura pronta.

## Plugins (C#) — xUnit + FakeXrmEasy
Projeto: `tests/Template.Plugins.Tests/` (mesmo TFM do assembly: **net462**).

- **FakeXrmEasy** simula o `IOrganizationService` e o **pipeline** (`XrmFakedContext`,
  `ExecutePluginWith<TPlugin>`), então cada plugin é testado **isolado**, como um método.
- Padrão de teste: Arrange (montar `Target`/imagens/`PluginContext`) → Act (`ExecutePluginWith`) → Assert.
- Cobertos no template: `AtualizarNomePlugin`, `AtualizarRelacionamentoPlugin`, `EntityRepository`.

```sh
dotnet test tests/Template.Plugins.Tests
```

> **Licença FakeXrmEasy:** a linha **1.x** (`FakeXrmEasy.9`) é MIT/grátis. As **v2/v3**
> exigem chave de licença. O template usa a 1.x; troque conforme sua necessidade.

> **Ambiente:** net462 roda no Windows ou via Mono/CI com .NET SDK. Não roda em Linux puro
> sem Mono — por isso a suíte C# **não** foi executada no container deste scaffolding (só cabeada).

## Web resources (TypeScript) — Jest + ts-jest
Co-localizados em `src/webresources/tpl/` (arquivos `*.test.ts`).

- Lógica de negócio em **módulos puros** (ex.: `src/format.ts`) → testável sem mockar o `Xrm`.
- Entry-points de formulário (`src/account/form.ts`) são **finos** e delegam ao módulo puro;
  testados mockando um `formContext` mínimo.
- Build com **esbuild** → bundle IIFE (`global-name=Tpl`); o JS de `dist/` é o que sobe.

```sh
cd src/webresources/tpl
npm ci
npm test         # jest  (✅ executado neste scaffolding: 5 testes verdes)
npm run typecheck
npm run build
```

## Princípio
Cada unidade testável = uma responsabilidade. Plugin fino + regra isolada + acesso a dados
atrás de `IRepository` torna tudo **mockável** — é o que destrava ter testes de verdade.
