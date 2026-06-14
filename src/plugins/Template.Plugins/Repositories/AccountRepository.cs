using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>
    /// Acesso a dados de <c>account</c>. **Toda query mora aqui** (não no service/plugin).
    /// Catálogo de exemplos: igualdade, Like, In, Money, OptionSet, filtro composto AND/OR,
    /// ordenação, relacionamento na query (link-entity) e N:N (associate).
    /// </summary>
    public sealed class AccountRepository : RepositoryBase
    {
        public AccountRepository(IOrganizationService service) : base(service) { }

        public Guid Create(Account account) => base.Create(account);

        public Account GetById(Guid id, params string[] columns) => RetrieveById<Account>(id, columns);

        /// <summary>Filtro simples: igualdade.</summary>
        public Account GetByName(string name)
        {
            var q = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name, Account.Fields.PrimaryContactId),
                TopCount = 1
            };
            q.Criteria.AddCondition(Account.Fields.Name, ConditionOperator.Equal, name);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Account>()).FirstOrDefault();
        }

        /// <summary>Like (busca por trecho) + ordenação ascendente.</summary>
        public IList<Account> SearchByNameLike(string term)
        {
            var q = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name)
            };
            q.Criteria.AddCondition(Account.Fields.Name, ConditionOperator.Like, $"%{term}%");
            q.AddOrder(Account.Fields.Name, OrderType.Ascending);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Account>()).ToList();
        }

        /// <summary>OptionSet (enum) com operador In.</summary>
        public IList<Account> FindByCategory(params AccountCategory[] categories)
        {
            var values = categories.Select(c => (object)(int)c).ToArray();
            var q = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name, Account.Fields.Category)
            };
            q.Criteria.AddCondition(Account.Fields.Category, ConditionOperator.In, values);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Account>()).ToList();
        }

        /// <summary>Money: receita ≥ mínimo, maiores primeiro (filtro + ordenação desc + Top).</summary>
        public IList<Account> TopByRevenue(decimal min, int top)
        {
            var q = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name, Account.Fields.Revenue),
                TopCount = top
            };
            q.Criteria.AddCondition(Account.Fields.Revenue, ConditionOperator.GreaterEqual, min);
            q.AddOrder(Account.Fields.Revenue, OrderType.Descending);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Account>()).ToList();
        }

        /// <summary>Filtro composto: ativo E (preferencial OU com muitos funcionários).</summary>
        public IList<Account> FindActivePreferredOrBig(int minEmployees)
        {
            var q = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name, Account.Fields.Category, Account.Fields.NumberOfEmployees)
            };
            q.Criteria.AddCondition(Account.Fields.StateCode, ConditionOperator.Equal, (int)AccountState.Active);

            var ou = new FilterExpression(LogicalOperator.Or);
            ou.AddCondition(Account.Fields.Category, ConditionOperator.Equal, (int)AccountCategory.PreferredCustomer);
            ou.AddCondition(Account.Fields.NumberOfEmployees, ConditionOperator.GreaterEqual, minEmployees);
            q.Criteria.AddFilter(ou);

            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Account>()).ToList();
        }

        /// <summary>
        /// Relacionamento N:1 na própria query (link-entity): traz a conta + o nome do contato
        /// principal de uma vez. O valor da coluna ligada vem como <see cref="AliasedValue"/>.
        /// </summary>
        public IDictionary<Guid, string> PrimaryContactNames()
        {
            var q = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name)
            };
            var link = q.AddLink(Contact.EntityLogicalName, Account.Fields.PrimaryContactId, Contact.Fields.ContactId, JoinOperator.LeftOuter);
            link.EntityAlias = "pc";
            link.Columns = new ColumnSet(Contact.Fields.FullName);

            return RetrieveMultiple(q).Entities.ToDictionary(
                e => e.Id,
                e => e.GetAttributeValue<AliasedValue>("pc." + Contact.Fields.FullName)?.Value as string);
        }

        /// <summary>Relacionamento N:N: associa contatos à conta (ex.: lista/segmento).</summary>
        public void AssociateContacts(Guid accountId, string relationshipName, IEnumerable<Guid> contactIds)
        {
            var refs = contactIds.Select(id => new EntityReference(Contact.EntityLogicalName, id)).ToArray();
            Associate(Account.EntityLogicalName, accountId, relationshipName, refs);
        }
    }
}
