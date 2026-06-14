using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>Acesso a dados de <c>account</c>. As queries da conta ficam aqui (não no service/plugin).</summary>
    public sealed class AccountRepository : RepositoryBase
    {
        public AccountRepository(IOrganizationService service) : base(service) { }

        public Guid Create(Account account) => base.Create(account);

        public Account GetById(Guid id, params string[] columns) => RetrieveById<Account>(id, columns);

        public Account GetByName(string name)
        {
            var query = new QueryExpression(Account.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Account.Fields.Name, Account.Fields.PrimaryContactId),
                TopCount = 1,
                Criteria = new FilterExpression(LogicalOperator.And)
            };
            query.Criteria.AddCondition(Account.Fields.Name, ConditionOperator.Equal, name);

            return RetrieveMultiple(query).Entities.Select(e => e.ToEntity<Account>()).FirstOrDefault();
        }
    }
}
