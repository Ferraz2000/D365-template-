---
title: Integrações: Service Bus e HTTP (async)
type: knowledge
area: plugins
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-padroes-plugin-comunidade.md
  - raw/sources/2026-06-14-ms-learn-alm-d365.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [plugins, integracao]
---

# Integrações: Service Bus e HTTP (async)

> Integração externa **sempre em step assíncrono**. Preferir **fila (Azure Service Bus)** ao HTTP direto.

## A decisão / conceito
- Desacoplado (recomendado): `context.PostarNaFila(serviceEndpointId)` via `IServiceEndpointNotificationService`.
- HTTP: `ClienteRest` (HttpClient injetável, com **retry**) — testável com handler falso.
- **Por quê:** step síncrono prende a transação; fila é resiliente e não acopla o CRM ao externo.

## Relacionadas
- [[padroes-de-plugin]]
