# GEMINI.md

Este repositório usa **`AGENTS.md`** como roteador único para agentes.

👉 Leia [`AGENTS.md`](./AGENTS.md) primeiro (estrutura, convenções, regra de dependência,
fluxo de PR e doc-sync).

Memória durável do projeto: `docs/brain/` (hipocampo). Padrão de arquitetura escrito:
`docs/architecture/`.

## Memória no Gemini CLI
- Comandos: `/search <termos>`, `/capture <algo>`, `/registra <algo>` (em `.gemini/commands/`).
- Briefing git on-demand: `python3 -m hipocampo.hooks.session_start`.
- O núcleo é Python puro (stdlib) — os mesmos CLIs valem aqui, no Claude e no Codex.
