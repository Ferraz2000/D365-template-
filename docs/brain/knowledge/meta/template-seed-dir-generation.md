---
title: Geração do template usa seed-dir (vault real intacto, seed curado para o consumidor)
type: knowledge
area: meta
status: active
confidence: high
provenance: inferred
sources:
created: 2026-06-17
updated: 2026-06-17
valid_until: ""
superseded_by: ""
tags: [knowledge, meta, dotnet-new, template, hipocampo, vault]
---

# Geração do template usa seed-dir

> **Verdade no brain.** O `dotnet new` **não** deve copiar o vault real do template
> (`docs/brain/`) para o projeto gerado — ele carregaria a história de decisão do
> template como se fosse do consumidor. A geração mapeia um **seed curado** e
> auto-consistente.

## Contexto

`docs/brain/` é ao mesmo tempo a memória de trabalho do template E a semente do
consumidor. Excluir notas por glob quebra o gate de integridade (index órfão,
`sources:` apontando pra arquivo sumido = FAIL em `vault_sync`/`doc_links`, que
**bloqueiam** push/CI). Editar o vault real corromperia a memória do template.

## A decisão / conceito

Mecanismo **seed-dir** em `.template.config/template.json`:

```jsonc
"sources": [
  { "exclude": ["docs/brain/**", "seed/**", ...], "rename": {"README.project.md": "README.md"} },
  { "source": "seed/brain/", "target": "docs/brain/" }
]
```

- Template mantém `docs/brain/` real intacto; geração entrega `seed/brain/` em
  `docs/brain/`.
- Seed **mantém**: knowledge `architecture`/`plugins`/`webresources` + fontes
  genéricas (`padroes-plugin-comunidade`, `ms-learn-alm`) + templates/capture/
  context-budget/README/index (trimmed).
- Seed **dropa** (história do template): ADRs, `knowledge/meta`, `knowledge/alm`,
  `raw/sources/...-sessao-decisoes.md`; `log.md` resetado; frontmatter/index limpos
  das citações órfãs.
- **Invariante:** o seed precisa passar `python -m hipocampo.gate ci` no projeto
  gerado (doc_links + vault_sync, 0 FAIL). `doc_links` valida o `seed/` no repo do
  template, então ele não apodrece em silêncio.

**Por quê:** consumidor começa com memória limpa + o padrão reutilizável; integridade
nunca quebra na primeira geração. **Trade-off:** `seed/brain` é cópia paralela curada
do knowledge — pode divergir do `docs/brain` real (esperado; é deliverable).

Também excluídos da geração (só-do-template): `.vs/`, `TEMPLATE.md`, `LICENSE`,
`CONTRIBUTING.md`, `.github/workflows/release.yml`; `README.md` do template trocado
por `README.project.md` (projeto privado, sem MIT).

## Relacionadas

- [[template-reutilizavel]]
- [[doc-sync-enforcement-gate]]
