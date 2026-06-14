using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Model;
using Template.Plugins.Repositories;

namespace Template.Plugins.Services
{
    /// <summary>
    /// Regra de negócio da conta. Use um service quando a regra cresce ou mexe em dados.
    /// As queries vêm dos repositórios (por entidade); aqui só a decisão de negócio.
    /// </summary>
    public sealed class AccountService
    {
        private readonly AccountRepository _accounts;
        private readonly ContactRepository _contacts;

        public AccountService(AccountRepository accounts, ContactRepository contacts)
        {
            Guard.AgainstNull(accounts, nameof(accounts));
            Guard.AgainstNull(contacts, nameof(contacts));
            _accounts = accounts;
            _contacts = contacts;
        }

        /// <summary>Regra que consome uma query: barra conta com nome já existente.</summary>
        public void RejeitarSeNomeDuplicado(Account account)
        {
            Guard.AgainstNull(account, nameof(account));
            if (string.IsNullOrWhiteSpace(account.Name)) return;

            var existente = _accounts.GetByName(account.Name);
            if (existente != null && existente.Id != account.Id)
                throw new InvalidPluginExecutionException($"Já existe uma conta com o nome '{account.Name}'.");
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
