using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Template.Plugins.Repositories
{
    /// <summary>
    /// Abstração de acesso a dados ao Dataverse. Plugins dependem DESTA interface,
    /// nunca de IOrganizationService cru — assim a regra de negócio fica testável e desacoplada.
    /// </summary>
    public interface IRepository
    {
        Guid Create(Entity entity);
        void Update(Entity entity);
        void Delete(string logicalName, Guid id);
        Entity Retrieve(string logicalName, Guid id, params string[] columns);

        /// <summary>Retrieve tipado (early-bound): o logical name vem de <typeparamref name="T"/>.</summary>
        T Retrieve<T>(Guid id, params string[] columns) where T : Entity, new();

        EntityCollection RetrieveMultiple(QueryBase query);
    }
}
