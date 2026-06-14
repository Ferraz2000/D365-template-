# ADR-0006: CI/CD eficiente (cache, concurrency, path filters)

- Status: Aceito
- Data: 2026-06-14

## Contexto
CI rodando tudo em todo push (inclusive o caro runner Windows) é lento e desperdiça minutos.

## Decisão
Jobs paralelos; cache NuGet/npm; concurrency cancel-in-progress; path filters (Windows so quando muda src/plugins|tests). Release publica no GitHub Packages via GITHUB_TOKEN (sem key); nuget.org opcional por secret.

## Consequências
Runs mais rapidos/baratos; o Windows so sobe quando necessario. Jobs irrelevantes ficam 'skipped' (cuidado se virarem required checks).
