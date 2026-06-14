# Usando este repositório como template

Para **começar projetos D365 CE novos** já com: vertical slice (Screaming), plugins C#,
web resources TypeScript, testes, CI, doc-sync e memória (Hipocampo).

## Opção A — `dotnet new` (instalável, recomendado)
```sh
# instala uma vez (a partir da pasta do template, ou de um pacote NuGet)
dotnet new install <caminho-do-template>     # ex.: dotnet new install .

# cria um PROJETO NOVO já renomeado
dotnet new d365ce -n Contoso.Crm                 # mantém o prefixo padrão "tpl"
dotnet new d365ce -n Contoso.Crm --prefix ctso   # já com o SEU prefixo de publisher
```
O que os parâmetros fazem:
- **`-n Contoso.Crm`** → renomeia namespace e assembly (`Template.Plugins` → `Contoso.Crm`), pastas e `.csproj`.
- **`--prefix ctso`** → troca o prefixo de schema custom (`tpl_servicos` → `ctso_servicos`, Custom API,
  pasta/global dos web resources). Default `tpl`. Centralizado em `Common.Publisher.Prefixo`.

> Só serve para **criar projetos novos** — não dá para "mergear" num projeto existente (é o objetivo).

## Opção B — GitHub Template (sem ferramenta)
Marque o repo em **Settings → Template repository**. Aí todo projeto novo nasce pelo botão
**"Use this template"** (cópia limpa). Depois renomeie o prefixo em `Common.Publisher.Prefixo` (um lugar só).

## ⚠️ O template NÃO injeta nada na sua organização
Os exemplos vêm **prontos e inertes**:
- `dotnet build` gera um **DLL no disco** — **não toca em org nenhuma**.
- Os testes rodam **em memória** (harness), **sem credencial** de Dataverse. A CI também.
- Um plugin só **entra em vigor** quando **você** registra o step (Plugin Registration Tool /
  `pac plugin` / import de solution) — ato **manual e opt-in**, plugin a plugin.

### Ativando um exemplo (quando quiser)
1. Crie no Dataverse o schema que o exemplo usa (ex.: colunas `tpl_servicos`/`tpl_resumo`,
   o Custom API `tpl_CalcularScoreConta`). Os de **campos padrão** (`name`, `revenue`…) já existem.
2. Compile e **registre o step** com a mensagem/estágio/entidade que está no comentário XML do plugin
   (ver `docs/architecture/plugins.md` → tabela de gatilhos).
3. Não precisa de um exemplo? É só **não registrar** (ou apagar o arquivo) — ele não faz nada sozinho.

## Depois de criar o projeto
1. **Testes:** `dotnet test tests/<Seu.Projeto>.Tests` e `cd src/webresources/<prefixo> && npm ci && npm test`.
2. **Ligar o gate doc-sync** (não viaja por git config):
   - No **Claude Code** liga sozinho (hook SessionStart). Fora dele: `git config core.hooksPath .githooks`.
3. **Publisher/prefixo:** defina no Dataverse igual ao `Common.Publisher.Prefixo`.
4. **URL de integração:** tire do hardcode (`IntegracaoPlugin`) → Environment Variable.
5. **Proteger `main`** e seguir o fluxo de PR (a CI roda testes + doc-sync).

## Memória (Hipocampo) no projeto novo
O brain **e o motor** vêm vendorizados (funcionam out-of-the-box); o **plugin** (atalhos) é à parte.
- **`/capture` e `/search`:** instale o plugin uma vez — `/plugin install hipocampo@hipocampo` ou
  `npx skills add Ferraz2000/hipocampo`. (O gate, validators e hooks já funcionam sem isso.)
- **Reset do brain por projeto:** as páginas `knowledge/` + `adrs/` são **convenções-semente genéricas** —
  **mantenha**. Já o `log.md` e o source da sessão são específicos — zere-os:
  ```sh
  : > docs/brain/log.md
  rm -f docs/brain/raw/sources/2026-06-14-sessao-decisoes.md
  # e remova a citação dessa fonte nas páginas que a usam, ou re-capture
  ```
  Depois capture as decisões do **seu** projeto via `/capture`.

## Publicar o template (pra qualquer um instalar por nome)
```sh
dotnet pack Template.Pack.csproj -c Release -o ./nupkg
dotnet nuget push ./nupkg/*.nupkg --api-key <SUA_KEY> --source https://api.nuget.org/v3/index.json
# depois, qualquer um:  dotnet new install D365CE.VerticalSlice.Template
```
Ou, sem NuGet: marque o repo como **GitHub Template** (Settings → Template repository).
