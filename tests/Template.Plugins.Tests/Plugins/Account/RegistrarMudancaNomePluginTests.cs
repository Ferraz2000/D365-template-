using System;
using System.Linq;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using RegistrarMudancaNomePlugin = Template.Plugins.Plugins.Account.RegistrarMudancaNomePlugin;

namespace Template.Plugins.Tests
{
    public class RegistrarMudancaNomePluginTests
    {
        [Fact]
        public void Post_operation_compara_preimage_com_target()
        {
            var harness = new PluginHarness();
            var id = Guid.NewGuid();

            var target = new Account(id) { Name = "Novo Nome" };
            var context = harness.Context(Messages.Update, Stages.PostOperation, Account.EntityLogicalName);
            context.InputParameters["Target"] = target;
            context.PreEntityImages["preimage"] = new Account(id) { Name = "Nome Antigo" };

            harness.Execute<RegistrarMudancaNomePlugin>(context);

            Assert.Contains(harness.Tracing.Logs, l => l.Contains("Nome Antigo") && l.Contains("Novo Nome"));
        }
    }
}
