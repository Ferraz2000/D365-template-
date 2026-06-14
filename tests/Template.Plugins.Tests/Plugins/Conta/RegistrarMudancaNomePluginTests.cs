using System;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class RegistrarMudancaNomePluginTests
    {
        [Fact]
        public void Post_operation_compara_preimage_com_target()
        {
            var harness = new PluginHarness();
            var id = Guid.NewGuid();

            var target = new Conta(id) { Nome = "Novo Nome" };
            var context = harness.Context(Messages.Update, Stages.PostOperation, Conta.EntityLogicalName);
            context.InputParameters["Target"] = target;
            context.PreEntityImages["preimage"] = new Conta(id) { Nome = "Nome Antigo" };

            harness.Execute<RegistrarMudancaNomePlugin>(context);

            Assert.Contains(harness.Tracing.Logs, l => l.Contains("Nome Antigo") && l.Contains("Novo Nome"));
        }
    }
}
