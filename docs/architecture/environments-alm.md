# Ambientes & ALM — padrão

> Como o código-fonte deste repo vira solution deployada, sem versionar a solution.

## Estratégia de ambientes
`Dev → Test → UAT → Prod`. Cada desenvolvedor no seu ambiente de Dev.
- **Dev**: unmanaged, mudanças em andamento.
- **Test/UAT/Prod**: recebem **managed** (nunca editar direto).

## Solutions
- Promover sempre como **managed**; managed é **artefato de build** gerado pela pipeline.
- Usar **environment variables** e **connection references** para portabilidade entre ambientes
  (nada de URL/segredo hard-coded).

## Fluxo de build/release (resumo)
1. Dev codifica em `src/` (plugins C#, web resources TS, PCF) e registra na solution unmanaged do Dev.
2. Build dos fontes: `dotnet build` (plugins) e `npm run build` (web resources/PCF).
3. Export da solution: `pac solution export --managed` → `.zip` (artefato, **fora do git** — `.gitignore` barra).
4. Import managed em Test/UAT/Prod (Power Platform Pipelines / Azure DevOps / GitHub Actions).

## Por que a solution não fica no repo
- O dump unpacked (centenas de XML de forms/views/entidades) é **pesado e ruidoso**.
- Este repo é a **fonte de verdade do código + do padrão**; a configuração de solution vive no
  ambiente e é exportada sob demanda como artefato.

## Próximos passos (fora do escopo deste skeleton)
- Pipeline de CI/CD (GitHub Actions ou Azure DevOps) para build dos fontes + export/import managed.
- Power Apps Checker (análise estática) no PR.
- Branching: feature → PR → `main`.
