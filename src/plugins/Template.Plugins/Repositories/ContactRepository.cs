using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>Implementação do acesso a dados de <see cref="Contact"/>.</summary>
    public sealed class ContactRepository : RepositoryBase, IContactRepository
    {
        public ContactRepository(IOrganizationService service) : base(service) { }

        public Contact GetById(Guid id, params string[] columns) => RetrieveById<Contact>(id, columns);

        public void Update(Contact contact) => base.Update(contact);
    }
}
