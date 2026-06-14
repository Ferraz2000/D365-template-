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
        private static AccountService NewService(FakeOrganizationService crm) =>
            new AccountService(new AccountRepository(crm), new ContactRepository(crm));

        [Fact]
        public void RejeitarSeNomeDuplicado_barra_nome_existente()
        {
            var crm = new FakeOrganizationService();
            new AccountRepository(crm).Create(new Account { Name = "Contoso" });

            var nova = new Account(Guid.NewGuid()) { Name = "Contoso" };

            Assert.Throws<InvalidPluginExecutionException>(() => NewService(crm).RejeitarSeNomeDuplicado(nova));
        }

        [Fact]
        public void RejeitarSeNomeDuplicado_permite_nome_unico()
        {
            var crm = new FakeOrganizationService();
            var ex = Record.Exception(() => NewService(crm).RejeitarSeNomeDuplicado(new Account(Guid.NewGuid()) { Name = "Fabrikam" }));
            Assert.Null(ex);
        }

        [Fact]
        public void PropagarContatoPrincipal_vincula_a_conta_no_contato()
        {
            var crm = new FakeOrganizationService();
            var contatoId = crm.Create(new Contact(Guid.NewGuid()));

            var account = new Account(Guid.NewGuid())
            {
                PrimaryContactId = new EntityReference(Contact.EntityLogicalName, contatoId)
            };

            NewService(crm).PropagarContatoPrincipal(account);

            var contato = crm.Retrieve(Contact.EntityLogicalName, contatoId, new ColumnSet(true)).ToEntity<Contact>();
            Assert.Equal(account.Id, contato.ParentCustomerId.Id);
        }
    }
}
