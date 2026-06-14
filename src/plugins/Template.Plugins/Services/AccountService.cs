using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Model;
using Template.Plugins.Repositories;

namespace Template.Plugins.Services
{
    /// <summary>
    /// Regra de negócio da conta. Use um service assim **quando a regra cresce ou mexe em dados**
    /// (aqui ela usa o repositório de contato). Regras triviais podem ficar no próprio plugin.
    /// </summary>
    public sealed class AccountService
    {
        private readonly ContactRepository _contacts;

        public AccountService(ContactRepository contacts)
        {
            Guard.AgainstNull(contacts, nameof(contacts));
            _contacts = contacts;
        }

        /// <summary>O contato principal da conta passa a tê-la como cliente-pai.</summary>
        public void PropagarContatoPrincipal(Account account)
        {
            Guard.AgainstNull(account, nameof(account));
            if (account.PrimaryContactId == null) return;

            var contato = new Contact(account.PrimaryContactId.Id)
            {
                ParentCustomerId = new EntityReference(Account.EntityLogicalName, account.Id)
            };
            _contacts.Update(contato);
        }
    }
}
