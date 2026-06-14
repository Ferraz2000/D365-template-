using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>Implementação do acesso a dados de <see cref="Account"/>.</summary>
    public sealed class AccountRepository : RepositoryBase, IAccountRepository
    {
        public AccountRepository(IOrganizationService service) : base(service) { }

        public Guid Create(Account account) => base.Create(account);

        public Account GetById(Guid id, params string[] columns) => RetrieveById<Account>(id, columns);

        public Account GetByName(string name)
        {
            // Query (QueryExpression) encapsulada no repositório.
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
