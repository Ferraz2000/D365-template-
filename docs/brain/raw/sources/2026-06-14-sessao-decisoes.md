---
title: Sessão de decisões do template D365 CE
type: source
area: meta
source_type: transcript
url: ""
provenance: external
status: active
retrieved: 2026-06-14
created: 2026-06-14
updated: 2026-06-14
tags: [source, sessao]
---

# Sessão de decisões do template D365 CE

> **Imutável.** Decisões tomadas com o owner durante a construção do template.

## Metadados
- Origem: conversa (chat) com o owner.
- Trazida em: 2026-06-14 via chat.

## Resumo fiel
- Produto: D365 Customer Engagement (Dataverse). Escopo: skeleton inicial, sem CI/CD.
- Repo guarda **só código-fonte + o padrão**; solutions exportadas não entram.
- Idioma: **domínio em PT, infra em EN**.
- Arquitetura: **vertical slice (Screaming)**; 1 plugin = 1 responsabilidade = 1 step.
- **Sem interfaces e sem DI** (júnior-friendly): dependências com `new`.
- Regra no service, query no repositório; entidades tipadas (early-bound).
- Testes: harness próprio (FakeXrmEasy crasha no Mono); target net462 rodado via Mono.
- Hipocampo vendorizado; team=true (PR + doc-sync gate).
