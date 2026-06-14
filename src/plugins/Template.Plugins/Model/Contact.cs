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

        public EntityReference ParentCustomerId
        {
            get => GetAttributeValue<EntityReference>(Fields.ParentCustomerId);
            set => SetAttributeValue(Fields.ParentCustomerId, value);
        }

        public Guid ContactId
        {
            get => GetAttributeValue<Guid>(Fields.ContactId);
            set { SetAttributeValue(Fields.ContactId, value); Id = value; }
        }

        public static class Fields
        {
            public const string ContactId = "contactid";
            public const string ParentCustomerId = "parentcustomerid";
        }
    }
}
