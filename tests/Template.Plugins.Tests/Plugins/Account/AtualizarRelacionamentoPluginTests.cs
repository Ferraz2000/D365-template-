using System;
using System.Linq;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;
using Template.Plugins.Plugins.Account;
using Xunit;

namespace Template.Plugins.Tests.Plugins.Account
{
    public class AtualizarRelacionamentoPluginTests
    {
        [Fact]
        public void Propaga_conta_para_o_contato_principal()
        {
            // Arrange — um contato já existente
            var ctx = new XrmFakedContext();
            var contatoId = Guid.NewGuid();
            ctx.Initialize(new[] { new Entity(Tables.Contact, contatoId) });

            var contaId = Guid.NewGuid();
            var target = new Entity(Tables.Account, contaId);
            target["primarycontactid"] = new EntityReference(Tables.Contact, contatoId);

            var pluginCtx = ctx.GetDefaultPluginContext();
            pluginCtx.MessageName = Messages.Update;
            pluginCtx.Stage = Stages.PostOperation;
            pluginCtx.PrimaryEntityId = contaId;
            pluginCtx.InputParameters = new ParameterCollection { { "Target", target } };

            // Act
            ctx.ExecutePluginWith<AtualizarRelacionamentoPlugin>(pluginCtx);

            // Assert — o contato passou a apontar para a conta
            var service = ctx.GetOrganizationService();
            var contato = service.Retrieve(Tables.Contact, contatoId, new ColumnSet("parentcustomerid"));
            var parent = contato.GetAttributeValue<EntityReference>("parentcustomerid");
            Assert.NotNull(parent);
            Assert.Equal(contaId, parent.Id);
        }
    }
}
