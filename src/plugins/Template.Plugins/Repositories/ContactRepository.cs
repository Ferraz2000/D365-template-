using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>Acesso a dados de <c>contact</c>. As queries de contato ficam aqui.</summary>
    public sealed class ContactRepository : RepositoryBase
    {
        public ContactRepository(IOrganizationService service) : base(service) { }

        public Contact GetById(Guid id, params string[] columns) => RetrieveById<Contact>(id, columns);

        public void Update(Contact contact) => base.Update(contact);
    }
}
