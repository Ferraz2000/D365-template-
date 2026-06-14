# AGENTS.md — testes do assembly

Roteador local. Leia antes o roteador raiz `AGENTS.md` e `docs/architecture/testing.md`.

## O que mora aqui
- `Fakes/FakeHarness.cs` — harness sem dependência externa: pipeline + `IOrganizationService` em memória
  (CRUD, query com filtros/ordem/multi-select, link-entity, N:N, `Execute(UpdateRequest)`/concorrência,
  Service Bus falso).
- `Arquitetura/ArquiteturaTests.cs` — regras de dependência (vertical slice) via reflection.
- Testes por camada: `Model/`, `Repositories/`, `Services/`, `Plugins/Conta/`, `Integracao/`, `Contas/`.

## Regras / comandos
- **Sem FakeXrmEasy** (crasha no Mono; v2/v3 exigem licença) — usa o harness próprio.
- net462: `dotnet test` no Windows/CI; em Linux roda via **Mono** (`mono xunit.console.exe`).
- Toda regra/plugin novo vem com teste; mantenha os testes de arquitetura passando.

## Conhecimento durável
`docs/brain/knowledge/architecture/testes.md`.
