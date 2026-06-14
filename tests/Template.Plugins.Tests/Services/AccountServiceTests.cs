using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Repositories;
using Template.Plugins.Services;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using Contact = Template.Plugins.Model.Contact;

namespace Template.Plugins.Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public void NormalizarNome_apara_espacos()
        {
            // Regra de negócio testada SEM pipeline e SEM Dataverse.
            var service = new AccountService(new ContactRepository(new FakeOrganizationService()));
            var account = new Account { Name = "  Acme  " };

            service.NormalizarNome(account);

            Assert.Equal("Acme", account.Name);
        }

        [Fact]
        public void PropagarContatoPrincipal_vincula_a_conta_no_contato()
        {
            var crm = new FakeOrganizationService();
            var contatoId = crm.Create(new Contact(Guid.NewGuid()));
            var service = new AccountService(new ContactRepository(crm));

            var account = new Account(Guid.NewGuid())
            {
                PrimaryContactId = new EntityReference(Contact.EntityLogicalName, contatoId)
            };

            service.PropagarContatoPrincipal(account);

            var contato = crm.Retrieve(Contact.EntityLogicalName, contatoId, new ColumnSet(true)).ToEntity<Contact>();
            Assert.Equal(account.Id, contato.ParentCustomerId.Id);
        }
    }
}
