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
- `dotnet test tests/<Seu.Projeto>.Tests` e `cd src/webresources/<prefixo> && npm ci && npm test`.
- Defina seu **Publisher/prefixo** no Dataverse (igual ao `Common.Publisher.Prefixo`).
- Tire a **URL de integração** do hardcode (`IntegracaoPlugin`) → Environment Variable.
- Reaproveite ou limpe o `docs/brain/` (os ADRs/knowledge são convenções-semente genéricas).
- Proteja `main` e siga o fluxo de PR (a CI roda testes + doc-sync).
