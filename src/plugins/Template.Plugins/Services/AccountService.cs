using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Model;
using Template.Plugins.Repositories;

namespace Template.Plugins.Services
{
    /// <summary>Regras de negócio da conta. Não conhece o pipeline; usa repositórios por entidade.</summary>
    public sealed class AccountService : IAccountService
    {
        private readonly IContactRepository _contacts;

        public AccountService(IContactRepository contacts)
        {
            Guard.AgainstNull(contacts, nameof(contacts));
            _contacts = contacts;
        }

        /// <summary>Regra: o nome da conta é gravado sem espaços nas pontas.</summary>
        public void NormalizarNome(Account account)
        {
            Guard.AgainstNull(account, nameof(account));
            if (string.IsNullOrWhiteSpace(account.Name)) return;
            account.Name = account.Name.Trim();
        }

        /// <summary>Regra: o contato principal da conta passa a tê-la como cliente-pai.</summary>
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

        /// <summary>Regra: enfileira a conta para integração externa (stub).</summary>
        public void EnfileirarIntegracao(Account account)
        {
            Guard.AgainstNull(account, nameof(account));
            // TODO: integração real (fila/serviço). Mantida como stub no template.
        }
    }
}
