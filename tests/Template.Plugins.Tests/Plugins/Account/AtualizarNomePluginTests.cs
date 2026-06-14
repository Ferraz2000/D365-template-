using System;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using AtualizarNomePlugin = Template.Plugins.Plugins.Account.AtualizarNomePlugin;

namespace Template.Plugins.Tests
{
    public class AtualizarNomePluginTests
    {
        [Fact]
        public void Normaliza_o_nome_da_conta()
        {
            var harness = new PluginHarness();
            var target = new Account(Guid.NewGuid()) { Name = "  Acme  " };

            var context = harness.Context(Messages.Update, Stages.PreOperation, Account.EntityLogicalName);
            context.InputParameters["Target"] = target;

            harness.Execute<AtualizarNomePlugin>(context);

            Assert.Equal("Acme", target.Name);
        }
    }
}
