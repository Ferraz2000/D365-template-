using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Template.Plugins.Contatos
{
    /// <summary>Entidade tipada (early-bound) da tabela <c>contact</c>.</summary>
    [EntityLogicalName("contact")]
    public partial class Contato : Entity
    {
        public const string EntityLogicalName = "contact";

        public Contato() : base(EntityLogicalName) { }
        public Contato(Guid id) : base(EntityLogicalName, id) { }

        public string NomeCompleto
        {
            get => GetAttributeValue<string>(Fields.NomeCompleto);
            set => SetAttributeValue(Fields.NomeCompleto, value);
        }

        /// <summary>Lookup polimórfico (Customer = account ou contact).</summary>
        public EntityReference ClientePaiId
        {
            get => GetAttributeValue<EntityReference>(Fields.ClientePaiId);
            set => SetAttributeValue(Fields.ClientePaiId, value);
        }

        public static class Fields
        {
            public const string ContatoId = "contactid";
            public const string NomeCompleto = "fullname";
            public const string ClientePaiId = "parentcustomerid";
        }
    }
}
