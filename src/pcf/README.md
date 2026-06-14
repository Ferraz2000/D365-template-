# PCF — Power Apps Component Framework

Controles de código (TypeScript) ficam aqui. Mantenha o mesmo espírito: básico, por feature,
**lógica pura testável**.

## Criar um controle
```sh
cd src/pcf
pac pcf init --namespace Tpl --name MeuControle --template field
npm install
npm run build
```

## Testes
A lógica pura (sem `ComponentFramework`) fica em `src/*.ts` e é testada com Jest
(ex.: `src/logic.ts` → `rotuloEstrelas`):
```sh
cd src/pcf
npm ci && npm test
```

O controle é empacotado na solution no release (artefato de build — não versionado;
o `.gitignore` barra `node_modules/`, `out/`, `*.zip`).
