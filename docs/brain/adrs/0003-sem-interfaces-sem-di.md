# ADR-0003: Sem interfaces e sem DI

- Status: Aceito
- Data: 2026-06-14

## Contexto
Interfaces + composition root pesavam para juniores e somavam arquivos sem ganho real no sandbox.

## Decisão
Classes concretas montadas com new; regra trivial no plugin; service só quando há regra/dados.

## Consequências
Menos cerimônia; ainda testável (repo sobre IOrganizationService fake). Pode reintroduzir DI depois sem reescrever.
