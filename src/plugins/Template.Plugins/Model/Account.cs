using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Template.Plugins.Model
{
    /// <summary>
    /// Entidade tipada (early-bound) da tabela <c>account</c>.
    /// O atributo <see cref="EntityLogicalNameAttribute"/> é exigido por <c>Entity.ToEntity&lt;T&gt;()</c>
    /// (mesmo padrão do código gerado pelo CrmSvcUtil / pac modelbuilder).
    /// </summary>
    [EntityLogicalName("account")]
    public partial class Account : Entity
    {
        public const string EntityLogicalName = "account";

        public Account() : base(EntityLogicalName) { }
        public Account(Guid id) : base(EntityLogicalName, id) { }

        public string Name
        {
            get => GetAttributeValue<string>(Fields.Name);
            set => SetAttributeValue(Fields.Name, value);
        }

        public EntityReference PrimaryContactId
        {
            get => GetAttributeValue<EntityReference>(Fields.PrimaryContactId);
            set => SetAttributeValue(Fields.PrimaryContactId, value);
        }

        public Guid AccountId
        {
            get => GetAttributeValue<Guid>(Fields.AccountId);
            set { SetAttributeValue(Fields.AccountId, value); Id = value; }
        }

        /// <summary>Nomes lógicos dos atributos — evita "magic strings".</summary>
        public static class Fields
        {
            public const string AccountId = "accountid";
            public const string Name = "name";
            public const string PrimaryContactId = "primarycontactid";
        }
    }
}
