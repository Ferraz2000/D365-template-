namespace Template.Plugins.Common
{
    /// <summary>
    /// Identidade do publisher. **`Prefixo` é o único lugar com o prefixo de schema custom** —
    /// todo nome custom (colunas, Custom API, web resources) deriva daqui. Troque em um só ponto
    /// (ou via `dotnet new ... --prefix ctso`). Schema padrão (name, revenue…) NÃO usa prefixo.
    /// </summary>
    public static class Publisher
    {
        public const string Prefixo = "tpl";
    }

    /// <summary>Nomes lógicos e estágios — centralizados para evitar "magic strings".</summary>
    public static class Messages
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }

    public static class Stages
    {
        public const int PreValidation = 10;
        public const int PreOperation = 20;
        public const int MainOperation = 30;  // usado por Custom API / Custom Action
        public const int PostOperation = 40;
    }

    /// <summary>Nomes de mensagens customizadas (Custom API / Action) do projeto.</summary>
    public static class CustomMessages
    {
        public const string CalcularScoreConta = Publisher.Prefixo + "_CalcularScoreConta";
    }

    /// <summary>Schema names das tabelas usadas pelos plugins (prefixo do publisher: tpl).</summary>
    public static class Tables
    {
        public const string Account = "account";
        public const string Contact = "contact";
        public const string Opportunity = "opportunity";
        public const string Case = "incident";
    }
}
