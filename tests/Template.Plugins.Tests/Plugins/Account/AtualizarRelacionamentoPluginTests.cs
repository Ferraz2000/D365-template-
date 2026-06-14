using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using Contact = Template.Plugins.Model.Contact;
using AtualizarRelacionamentoPlugin = Template.Plugins.Plugins.Account.AtualizarRelacionamentoPlugin;

namespace Template.Plugins.Tests
{
    public class AtualizarRelacionamentoPluginTests
    {
        [Fact]
        public void Propaga_conta_para_o_contato_principal()
        {
            // Arrange — um contato já existente no "Dataverse" fake
            var harness = new PluginHarness();
            var contatoId = harness.Service.Create(new Contact(Guid.NewGuid()));

            var contaId = Guid.NewGuid();
            var target = new Account(contaId)
            {
                PrimaryContactId = new EntityReference(Contact.EntityLogicalName, contatoId)
            };

            var context = harness.Context(Messages.Update, Stages.PostOperation);
            context.PrimaryEntityId = contaId;
            context.InputParameters["Target"] = target;

            // Act
            harness.Execute<AtualizarRelacionamentoPlugin>(context);

            // Assert — o contato passou a apontar para a conta
            var contato = harness.Service
                .Retrieve(Contact.EntityLogicalName, contatoId, new ColumnSet(true))
                .ToEntity<Contact>();

            Assert.NotNull(contato.ParentCustomerId);
            Assert.Equal(contaId, contato.ParentCustomerId.Id);
        }
    }
}
