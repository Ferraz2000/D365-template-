using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Template.Plugins.Model
{
    /// <summary>
    /// Entidade tipada (early-bound) da tabela <c>account</c>. Mostra os tipos mais comuns do D365:
    /// texto, lookup (EntityReference), Money, OptionSet (enum), state, inteiro e data/hora.
    /// </summary>
    [EntityLogicalName("account")]
    public partial class Account : Entity
    {
        public const string EntityLogicalName = "account";

        public Account() : base(EntityLogicalName) { }
        public Account(Guid id) : base(EntityLogicalName, id) { }

        /// <summary>Texto.</summary>
        public string Name
        {
            get => GetAttributeValue<string>(Fields.Name);
            set => SetAttributeValue(Fields.Name, value);
        }

        /// <summary>Lookup N:1 (relacionamento) → EntityReference.</summary>
        public EntityReference PrimaryContactId
        {
            get => GetAttributeValue<EntityReference>(Fields.PrimaryContactId);
            set => SetAttributeValue(Fields.PrimaryContactId, value);
        }

        /// <summary>Money → expõe como decimal (o SDK guarda em <see cref="Money"/>).</summary>
        public decimal? Revenue
        {
            get => GetAttributeValue<Money>(Fields.Revenue)?.Value;
            set => SetAttributeValue(Fields.Revenue, value.HasValue ? new Money(value.Value) : null);
        }

        /// <summary>OptionSet → expõe como enum (o SDK guarda em <see cref="OptionSetValue"/>).</summary>
        public AccountCategory? Category
        {
            get
            {
                var o = GetAttributeValue<OptionSetValue>(Fields.Category);
                return o == null ? (AccountCategory?)null : (AccountCategory)o.Value;
            }
            set => SetAttributeValue(Fields.Category, value.HasValue ? new OptionSetValue((int)value.Value) : null);
        }

        /// <summary>statecode (ativo/inativo) → enum.</summary>
        public AccountState? State
        {
            get
            {
                var o = GetAttributeValue<OptionSetValue>(Fields.StateCode);
                return o == null ? (AccountState?)null : (AccountState)o.Value;
            }
            set => SetAttributeValue(Fields.StateCode, value.HasValue ? new OptionSetValue((int)value.Value) : null);
        }

        /// <summary>Inteiro.</summary>
        public int? NumberOfEmployees
        {
            get => GetAttributeValue<int?>(Fields.NumberOfEmployees);
            set => SetAttributeValue(Fields.NumberOfEmployees, value);
        }

        /// <summary>Data/hora (UTC no Dataverse).</summary>
        public DateTime? LastOnHoldTime
        {
            get => GetAttributeValue<DateTime?>(Fields.LastOnHoldTime);
            set => SetAttributeValue(Fields.LastOnHoldTime, value);
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
            public const string Revenue = "revenue";
            public const string Category = "accountcategorycode";
            public const string StateCode = "statecode";
            public const string NumberOfEmployees = "numberofemployees";
            public const string LastOnHoldTime = "lastonholdtime";
        }
    }
}
