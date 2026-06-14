# ADR-0001: Vertical slice (Screaming) por feature

- Status: Aceito
- Data: 2026-06-14

## Contexto
Organização por camada técnica não grita o domínio e mistura entidades quando há mais de uma.

## Decisão
Organizar o assembly por feature (namespace = feature): Contas/, Contatos/; infra em Common/ e Integracao/.

## Consequências
Domínio explícito e escalável; a separação de camadas vira convenção garantida por testes de arquitetura.
