# AGENTS.md — feature `Contatos` (namespace `Template.Plugins.Contatos`)

Roteador local. Leia antes o roteador raiz `AGENTS.md` e `docs/architecture/plugins.md`.

## O que mora aqui
- `Contato.cs` — entidade tipada (early-bound).
- `ContatoRepositorio.cs` — acesso a dados / queries de contato.

## Regras
- Feature **base**: **não pode depender de `Contas`** (a dependência é `Contas → Contatos`).
- Entidades tipadas; query só no repositório; herda `RepositoryBase` (em `Common`).
- Mudou `src/plugins/**`? Atualize `docs/architecture/plugins.md` no mesmo commit (doc-sync).
