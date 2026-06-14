using System;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class PublicarEventoContaPluginTests
    {
        [Fact]
        public void Publica_na_fila_via_notification_service()
        {
            var harness = new PluginHarness();
            var id = Guid.NewGuid();
            var target = new Conta(id);

            var context = harness.Context(Messages.Create, Stages.PostOperation, Conta.EntityLogicalName);
            context.PrimaryEntityId = id;
            context.InputParameters["Target"] = target;

            harness.Execute<PublicarEventoContaPlugin>(context);

            Assert.Equal(1, harness.Notifications.Chamadas);
        }
    }
}
