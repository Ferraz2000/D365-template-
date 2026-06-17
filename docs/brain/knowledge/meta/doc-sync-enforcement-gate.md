---
title: Doc-sync enforcement via [enforcement] por ponto (não flag global)
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
tags: [knowledge, meta, hipocampo, doc-sync, ci]
---

# Doc-sync enforcement via `[enforcement]` por ponto

> **Verdade no brain.** O doc-sync é forcing function **não-bloqueante** para o dev
> humano, mas a **integridade** do vault continua travando push/CI. Isso é configurado
> pelo mecanismo upstream `[enforcement]` do hipocampo (kit ≥ 0.9.0), não por hack local.

## Contexto

Antes (kit 0.8.3) havia um patch local `doc_sync_enforce` em `config.py` +
`feature_doc_sync.py` que tornava o doc-sync advisory. O kit 0.9.0 introduziu
`[enforcement]` + `hipocampo/gate.py` (política por ponto: `block|warn|off`),
tornando o hack redundante. Carregar patch em código vendored colide a cada
`brain-update`.

## A decisão / conceito

Migrado para o mecanismo upstream. `brain.config.toml`:

```toml
[enforcement]
pre_commit = "warn"   # doc-sync avisa no commit, nunca trava o dev
pre_push   = "block"  # integridade (doc_links/vault_sync) trava o push
ci         = "block"  # idem no merge
```

- Git hooks chamam `python -m hipocampo.gate <ponto>` (não os validators direto).
- `gate` é **por ponto, não por validator**: `pre-commit` roda só `feature_doc_sync`;
  `pre-push`/`ci` rodam o preflight completo.
- Logo `validators = ["doc_links", "vault_sync"]` (sem `feature_doc_sync`) — assim
  push/CI bloqueiam só integridade real; o lembrete de doc-sync fica no pre-commit
  como advisory.

**Por quê:** dev humano não pode travar por doc desatualizada (falta de tempo), mas
link morto / proveniência quebrada não pode entrar no `main`. **Trade-off:** push/CI
deixam de *imprimir* o lembrete de doc-sync (só o pre-commit imprime) — aceitável, é
onde o aviso é mais acionável.

## Relacionadas

- [[ci-cd-e-release]]
- [[template-seed-dir-generation]]
