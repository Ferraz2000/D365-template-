# Persona / preferências (seed)

- Idioma de trabalho: **pt-BR**.
- Arquitetura: **Screaming Architecture + Clean Code**, mantida **básica** — não
  super-engenheirar. Três blocos no assembly de plugin: `Plugins` / `Common` / `Repositories`.
- **1 plugin = 1 responsabilidade = 1 step** (como um método). Nunca acoplar várias ações
  de uma tabela na mesma classe.
- Repo guarda **só código-fonte + o padrão**. Solutions exportadas do D365 **não** entram
  no repositório (pesam demais).
- Time: fluxo de **PR**; doc e código andam juntos (doc-sync gate).
- Memória: capturar decisões/lições no vault (`/capture`), curadoria humana antes de virar
  conhecimento durável.
