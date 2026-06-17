# Usando este repositório como template

Para **começar projetos D365 CE novos** já com: vertical slice (Screaming), plugins C#,
web resources TypeScript, testes, CI, doc-sync e memória (Hipocampo).

## Opção A — `dotnet new` (instalável, recomendado)
```sh
# instala uma vez (do nuget.org, ou de um clone local)
dotnet new install D365CE.VerticalSlice.Template   # publicado no nuget.org (público, sem auth)
dotnet new install .                               # ou a partir do clone local (dev)

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
  `npx skills add Ferraz2000/hipocampo-memory`. (O gate, validators e hooks já funcionam sem isso.)
- **Vault já vem limpo:** o projeto gerado recebe um vault-semente **curado** (knowledge
  `architecture`/`plugins`/`webresources` + convenções genéricas; **sem** ADRs/`meta`/`alm`/sessão
  do template, `log.md` zerado). A geração mapeia `seed/brain → docs/brain` — **não precisa resetar
  nada à mão**. Só capture as decisões do **seu** projeto via `/capture`.

## Publicar o template (instalação por nome)
**Automático, sem API key:** crie um **Release no GitHub** (tag tipo `v1.0.0`) → o workflow
`.github/workflows/release.yml` empacota e publica no **GitHub Packages** usando o `GITHUB_TOKEN`
(não precisa de key nenhuma sua).

Para instalar de outra máquina (GitHub Packages pede autenticar a feed uma vez):
```sh
dotnet nuget add source https://nuget.pkg.github.com/Ferraz2000/index.json \
  -n github -u <seu-usuario-github> -p <PAT-com-read:packages> --store-password-in-clear-text
dotnet new install D365CE.VerticalSlice.Template
```

**Instalação pública (recomendado p/ repo open source) — nuget.org via Trusted Publishing (OIDC, sem API key longeva):**
qualquer um instala com `dotnet new install D365CE.VerticalSlice.Template`, **sem autenticar**. Habilite uma vez:
1. No nuget.org → menu do usuário → **Trusted Publishing** → nova policy:
   Repository Owner `Ferraz2000`, Repository `D365-template-`, Workflow File `release.yml` (só o nome, sem o caminho), Environment vazio.
2. Crie a secret com teu **username** do nuget.org (perfil, não e-mail): `gh secret set NUGET_USER`.
3. Publique: `gh workflow run release.yml -f version=1.1.1` (ou crie um Release/tag). O `NuGet/login@v1`
   troca o token OIDC do GitHub por uma key temporária (~1h); sem a secret/policy, o passo nuget.org é pulado.

> Não precisa mais de API key em secret. Policy de repo privado começa "ativa por 7 dias" até o 1º publish
> (fixa nos IDs do repo depois); repo público ativa direto.

**Sem NuGet:** botão **"Use this template"** (repo marcado como Template) — o caminho mais simples.
