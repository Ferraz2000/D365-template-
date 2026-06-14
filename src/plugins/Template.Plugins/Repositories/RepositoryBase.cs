using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;

namespace Template.Plugins.Repositories
{
    /// <summary>
    /// Base dos repositórios por entidade: encapsula o <see cref="IOrganizationService"/> e o CRUD
    /// comum. As **queries específicas vivem nos repositórios filhos** — nunca nos services/plugins.
    /// </summary>
    public abstract class RepositoryBase
    {
        protected IOrganizationService Service { get; }

        protected RepositoryBase(IOrganizationService service)
        {
            Guard.AgainstNull(service, nameof(service));
            Service = service;
        }

        protected Guid Create(Entity entity)
        {
            Guard.AgainstNull(entity, nameof(entity));
            return Service.Create(entity);
        }

        protected void Update(Entity entity)
        {
            Guard.AgainstNull(entity, nameof(entity));
            Service.Update(entity);
        }

        protected void Delete(string logicalName, Guid id)
        {
            Guard.AgainstNullOrEmpty(logicalName, nameof(logicalName));
            Service.Delete(logicalName, id);
        }

        protected T RetrieveById<T>(Guid id, params string[] columns) where T : Entity, new()
        {
            var logicalName = new T().LogicalName;
            var cs = (columns == null || columns.Length == 0) ? new ColumnSet(true) : new ColumnSet(columns);
            return Service.Retrieve(logicalName, id, cs).ToEntity<T>();
        }

        protected EntityCollection RetrieveMultiple(QueryBase query)
        {
            Guard.AgainstNull(query, nameof(query));
            return Service.RetrieveMultiple(query);
        }

        // Relacionamento N:N — associar/desassociar registros.
        protected void Associate(string entityName, Guid entityId, string relationshipName, params EntityReference[] related)
            => Service.Associate(entityName, entityId, new Relationship(relationshipName), new EntityReferenceCollection(related));

        protected void Disassociate(string entityName, Guid entityId, string relationshipName, params EntityReference[] related)
            => Service.Disassociate(entityName, entityId, new Relationship(relationshipName), new EntityReferenceCollection(related));
    }
}
