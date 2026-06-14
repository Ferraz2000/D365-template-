namespace Template.Plugins.Common
{
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
        public const int PostOperation = 40;
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
