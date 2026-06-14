---
title: Solutions fora do repo; managed; ALM
type: knowledge
area: alm
status: active
confidence: high
provenance: inferred
sources:
  - raw/sources/2026-06-14-ms-learn-alm-d365.md
  - raw/sources/2026-06-14-sessao-decisoes.md
created: 2026-06-14
updated: 2026-06-14
valid_until: ""
superseded_by: ""
tags: [alm, solutions]
---

# Solutions fora do repo; managed; ALM

> O repo guarda **código-fonte**; a solution vive no ambiente e é exportada como **artefato de build**.
> `.gitignore` barra exports (`*.zip`, unpacked).

## A decisão / conceito
- Promover sempre como **managed** para Test/UAT/Prod; um publisher/prefixo.
- Fronteiras: Core + features (Sales/Service) dependentes do Core.
- **Por quê:** dump unpacked é pesado/ruidoso; o padrão fica documentado, não os arquivos.

## Relacionadas
- [[escopo-e-stack]]
