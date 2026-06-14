# ADR-0004: Testes com harness próprio (sem FakeXrmEasy)

- Status: Aceito
- Data: 2026-06-14

## Contexto
FakeXrmEasy crasha o Mono; v2/v3 exigem licença.

## Decisão
Harness de fakes no repo simulando pipeline e IOrganizationService (CRUD/query/link-entity/N:N/concorrência); net462 roda via Mono.

## Consequências
Suíte roda em qualquer lugar, sem licença. FakeXrmEasy fica como opção para cenários ricos no CI.
