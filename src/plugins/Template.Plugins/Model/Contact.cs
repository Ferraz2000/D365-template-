using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Template.Plugins.Model
{
    /// <summary>Entidade tipada (early-bound) da tabela <c>contact</c>.</summary>
    [EntityLogicalName("contact")]
    public partial class Contact : Entity
    {
        public const string EntityLogicalName = "contact";

        public Contact() : base(EntityLogicalName) { }
        public Contact(Guid id) : base(EntityLogicalName, id) { }

        public string FullName
        {
            get => GetAttributeValue<string>(Fields.FullName);
            set => SetAttributeValue(Fields.FullName, value);
        }

        /// <summary>Lookup polimórfico (Customer = account ou contact).</summary>
        public EntityReference ParentCustomerId
        {
            get => GetAttributeValue<EntityReference>(Fields.ParentCustomerId);
            set => SetAttributeValue(Fields.ParentCustomerId, value);
        }

        public static class Fields
        {
            public const string ContactId = "contactid";
            public const string FullName = "fullname";
            public const string ParentCustomerId = "parentcustomerid";
        }
    }
}
