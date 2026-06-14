# Log — append-only

Registro cronológico de ingests e decisões duráveis (padrão Karpathy LLM-wiki). 1
linha por evento, mais recente no topo. Barato de anexar, nunca reescrito — é o
companheiro "o que aconteceu quando" das páginas curadas em `knowledge/`.

Formato: `## [AAAA-MM-DD] <tipo> | <título>` e uma linha opcional com links.

<!-- Exemplo:
## [2026-06-12] ingest | Pesquisa de frameworks de memória de agentes
Gravado raw/sources/2026-06-12-....md; tocou knowledge/meta/....md.
-->

## [2026-06-14] decisão | Template D365 CE: vertical slice (Screaming), PT/EN, sem interfaces
Decisões registradas em adrs/0001..0005 e knowledge/{architecture,plugins,webresources,alm,meta}.
Fonte: raw/sources/2026-06-14-sessao-decisoes.md.

## [2026-06-14] decisão | Testes com harness próprio (FakeXrmEasy crasha no Mono); net462 via Mono
Ver knowledge/architecture/testes.md (ADR-0004).

## [2026-06-14] decisão | Integração async: Service Bus (recomendado) e HTTP com retry
Ver knowledge/plugins/integracoes.md.

## [2026-06-14] decisão | CI/CD eficiente (cache, concurrency, path filters) + release no GitHub Packages
Ver knowledge/alm/ci-cd-e-release.md (ADR-0006).

## [2026-06-14] decisão | Template reutilizável (dotnet new + prefixo centralizado), MIT/público
Ver knowledge/meta/template-reutilizavel.md (ADR-0007). Não injeta plugins na org (registro opt-in).
