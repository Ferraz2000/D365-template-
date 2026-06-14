using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;

namespace Template.Plugins.Repositories
{
    /// <summary>Implementação de IRepository sobre o IOrganizationService do Dataverse.</summary>
    public sealed class EntityRepository : IRepository
    {
        private readonly IOrganizationService _service;

        public EntityRepository(IOrganizationService service)
        {
            Guard.AgainstNull(service, nameof(service));
            _service = service;
        }

        public Guid Create(Entity entity)
        {
            Guard.AgainstNull(entity, nameof(entity));
            return _service.Create(entity);
        }

        public void Update(Entity entity)
        {
            Guard.AgainstNull(entity, nameof(entity));
            _service.Update(entity);
        }

        public void Delete(string logicalName, Guid id)
        {
            Guard.AgainstNullOrEmpty(logicalName, nameof(logicalName));
            _service.Delete(logicalName, id);
        }

        public Entity Retrieve(string logicalName, Guid id, params string[] columns)
        {
            Guard.AgainstNullOrEmpty(logicalName, nameof(logicalName));
            var cs = (columns == null || columns.Length == 0) ? new ColumnSet(true) : new ColumnSet(columns);
            return _service.Retrieve(logicalName, id, cs);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            Guard.AgainstNull(query, nameof(query));
            return _service.RetrieveMultiple(query);
        }
    }
}
