# PCF — Power Apps Component Framework

Controles de código (TypeScript) ficam aqui. Mantenha o mesmo espírito: básico, por feature.

## Criar um controle
```sh
cd src/pcf
pac pcf init --namespace Tpl --name MeuControle --template field
npm install
npm run build
```

O controle é empacotado na solution no release (artefato de build — não versionado;
o `.gitignore` barra `node_modules/`, `out/`, `*.zip`).
