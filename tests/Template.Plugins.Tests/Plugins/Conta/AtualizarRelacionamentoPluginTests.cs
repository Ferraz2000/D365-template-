using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class AtualizarRelacionamentoPluginTests
    {
        [Fact]
        public void Propaga_conta_para_o_contato_principal()
        {
            var harness = new PluginHarness();
            var contatoId = harness.Service.Create(new Contato(Guid.NewGuid()));

            var contaId = Guid.NewGuid();
            var target = new Conta(contaId)
            {
                ContatoPrincipalId = new EntityReference(Contato.EntityLogicalName, contatoId)
            };

            var context = harness.Context(Messages.Update, Stages.PostOperation, Conta.EntityLogicalName);
            context.PrimaryEntityId = contaId;
            context.InputParameters["Target"] = target;

            harness.Execute<AtualizarRelacionamentoPlugin>(context);

            var contato = harness.Service
                .Retrieve(Contato.EntityLogicalName, contatoId, new ColumnSet(true))
                .ToEntity<Contato>();
            Assert.Equal(contaId, contato.ClientePaiId.Id);
        }
    }
}
