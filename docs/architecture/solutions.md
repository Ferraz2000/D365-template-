# Solutions & Publisher — padrão (sem versionar a solution)

> Este doc descreve as **fronteiras de solution**. Os arquivos da solution **não**
> ficam no repo (são pesados/ruidosos); o `.gitignore` barra exports. A solution vive
> no ambiente Dataverse e é exportada só como artefato de build (ver `environments-alm.md`).

## Publisher
- **Um único publisher** para todos os componentes. Nome significativo, prefixo curto.
- Placeholder do template: publisher **`Template`**, prefixo **`tpl`**.
- **Nunca** o `new_` default. O prefixo é praticamente **imutável** — escolha pensando em anos.
- Manter o número de publishers no mínimo (idealmente 1) evita conflitos de import ao mover componentes.

## Segmentação de solutions (fronteiras lógicas)
| Solution | Conteúdo | Depende de |
|----------|----------|------------|
| **Core** | Schema compartilhado: extensões de Account/Contact, tabelas base, env vars, connection references | — |
| **Sales** | Componentes de vendas (Opportunity, etc.) | Core |
| **Service** | Componentes de atendimento (Case, etc.) | Core |

- Fronteiras por **necessidade funcional**. Componentes compartilhados → Core.
- Mover componente entre solutions exige **mesmo publisher** (senão conflita no import).

## Managed vs Unmanaged
- **Unmanaged só em Dev** (enquanto há mudança).
- Promover para Test/UAT/Prod sempre como **managed** (artefato de build, nunca editado direto).

## Como o código-fonte se relaciona com a solution
- `src/plugins/` compila um **assembly** → registrado como componente da solution.
- `src/webresources/` compila TS→JS → web resources da solution.
- Na hora do release: `pac solution export`/`pack managed` empacota tudo. Esse pacote é
  artefato de build (fora do git).
