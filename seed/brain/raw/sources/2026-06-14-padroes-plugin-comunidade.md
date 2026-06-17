---
title: Padrões de plugin D365 (comunidade)
type: source
area: plugins
source_type: article
url: https://dev.to/mohamed_elgharably_9f7168/stop-writing-plugins-like-its-2011-modern-architecture-guide-1j9g
provenance: external
status: active
retrieved: 2026-06-14
created: 2026-06-14
updated: 2026-06-14
tags: [source, plugins]
---

# Padrões de plugin D365 (comunidade)

> **Imutável.** Síntese de fontes da comunidade sobre arquitetura moderna de plugins.

## Resumo fiel
- Plugin fino orquestra; regra em service; queries em repositório (camadas).
- PluginBase com RegisteredEvents (message/stage/entity) é o padrão clássico do SDK.
- Evitar framework de DI pesado no sandbox; usar factory simples / `new`.
- FakeXrmEasy: v1 MIT; v2/v3 exigem licença.
