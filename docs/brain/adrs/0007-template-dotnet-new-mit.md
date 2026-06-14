# ADR-0007: Distribuir como dotnet new template, MIT/publico

- Status: Aceito
- Data: 2026-06-14

## Contexto
Reuso precisa ser facil, instalavel, so para projetos novos, sem injetar nada em org.

## Decisão
dotnet new template (.template.config) com -n e --prefix; prefixo centralizado em Common.Publisher.Prefixo; pack via Template.Pack.csproj; licenca MIT + disclaimer Microsoft. Alternativa: GitHub Template.

## Consequências
Qualquer um cria projeto novo renomeado em 1 comando; nada é injetado na org (registro opt-in). Publicacao (visibility/release) é acao manual do dono.
