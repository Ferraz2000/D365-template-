using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Template.Plugins.Contas
{
    /// <summary>
    /// Entidade tipada (early-bound) da tabela <c>account</c>. Nome da classe em PT; o logical name
    /// e os nomes de atributo (em <see cref="Fields"/>) são fixos da plataforma.
    /// </summary>
    [EntityLogicalName("account")]
    public partial class Conta : Entity
    {
        public const string EntityLogicalName = "account";

        public Conta() : base(EntityLogicalName) { }
        public Conta(Guid id) : base(EntityLogicalName, id) { }

        public string Nome
        {
            get => GetAttributeValue<string>(Fields.Nome);
            set => SetAttributeValue(Fields.Nome, value);
        }

        /// <summary>Lookup N:1 → EntityReference.</summary>
        public EntityReference ContatoPrincipalId
        {
            get => GetAttributeValue<EntityReference>(Fields.ContatoPrincipalId);
            set => SetAttributeValue(Fields.ContatoPrincipalId, value);
        }

        /// <summary>Money → decimal.</summary>
        public decimal? Receita
        {
            get => GetAttributeValue<Money>(Fields.Receita)?.Value;
            set => SetAttributeValue(Fields.Receita, value.HasValue ? new Money(value.Value) : null);
        }

        /// <summary>OptionSet → enum.</summary>
        public CategoriaConta? Categoria
        {
            get
            {
                var o = GetAttributeValue<OptionSetValue>(Fields.Categoria);
                return o == null ? (CategoriaConta?)null : (CategoriaConta)o.Value;
            }
            set => SetAttributeValue(Fields.Categoria, value.HasValue ? new OptionSetValue((int)value.Value) : null);
        }

        /// <summary>statecode → enum.</summary>
        public EstadoConta? Estado
        {
            get
            {
                var o = GetAttributeValue<OptionSetValue>(Fields.Estado);
                return o == null ? (EstadoConta?)null : (EstadoConta)o.Value;
            }
            set => SetAttributeValue(Fields.Estado, value.HasValue ? new OptionSetValue((int)value.Value) : null);
        }

        /// <summary>Multi-select OptionSet → array de enum (guarda em <see cref="OptionSetValueCollection"/>).</summary>
        public Servico[] Servicos
        {
            get
            {
                var col = GetAttributeValue<OptionSetValueCollection>(Fields.Servicos);
                return col == null ? new Servico[0] : col.Select(o => (Servico)o.Value).ToArray();
            }
            set => SetAttributeValue(Fields.Servicos,
                (value == null || value.Length == 0)
                    ? null
                    : new OptionSetValueCollection(value.Select(s => new OptionSetValue((int)s)).ToList()));
        }

        public int? NumeroDeFuncionarios
        {
            get => GetAttributeValue<int?>(Fields.NumeroDeFuncionarios);
            set => SetAttributeValue(Fields.NumeroDeFuncionarios, value);
        }

        public DateTime? UltimaEspera
        {
            get => GetAttributeValue<DateTime?>(Fields.UltimaEspera);
            set => SetAttributeValue(Fields.UltimaEspera, value);
        }

        /// <summary>Texto derivado (usado no exemplo de anti-loop).</summary>
        public string Resumo
        {
            get => GetAttributeValue<string>(Fields.Resumo);
            set => SetAttributeValue(Fields.Resumo, value);
        }

        public Guid ContaId
        {
            get => GetAttributeValue<Guid>(Fields.ContaId);
            set { SetAttributeValue(Fields.ContaId, value); Id = value; }
        }

        public static class Fields
        {
            public const string ContaId = "accountid";
            public const string Nome = "name";
            public const string ContatoPrincipalId = "primarycontactid";
            public const string Receita = "revenue";
            public const string Categoria = "accountcategorycode";
            public const string Estado = "statecode";
            public const string NumeroDeFuncionarios = "numberofemployees";
            public const string UltimaEspera = "lastonholdtime";
            public const string Servicos = "tpl_servicos";   // multi-select
            public const string Resumo = "tpl_resumo";
        }
    }
}
