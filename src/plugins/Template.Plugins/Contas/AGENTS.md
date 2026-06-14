# AGENTS.md — feature `Contas` (namespace `Template.Plugins.Contas`)

Roteador local. Leia antes o roteador raiz `AGENTS.md` e `docs/architecture/plugins.md`.

## O que mora aqui (vertical slice)
- `Conta.cs` / `ContaEnums.cs` — entidade tipada (early-bound) + enums (Categoria/Estado/Servico).
- `ContaRepositorio.cs` — **todas as queries da conta** (igualdade, Like, In, Money, multi-select,
  link-entity N:1, N:N, concorrência otimista).
- `ContaServico.cs` — regra de negócio (só quando há regra/dados).
- `ContaPayload.cs` — payload de integração (puro).
- `*Plugin.cs` — ações; **1 plugin = 1 step**.

## Regras
- Plugin herda `PluginBase` e implementa `Execute`; regra trivial fica no plugin, senão vai pro service.
- **Query só no repositório.** Entidades tipadas (`conta.Nome`, não `entity["..."]`).
- Pode depender de `Contatos` e de `Common`/`Integracao` — **nunca o contrário**.
- Mudou algo em `src/plugins/**`? Atualize `docs/architecture/plugins.md` no mesmo commit (doc-sync).

## Conhecimento durável
`docs/brain/knowledge/plugins/` e `docs/brain/knowledge/architecture/vertical-slice-screaming.md`.
