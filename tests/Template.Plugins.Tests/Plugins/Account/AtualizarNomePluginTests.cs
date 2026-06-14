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
        public void Executa_no_step_registrado_e_normaliza()
        {
            var harness = new PluginHarness();
            var target = new Account(Guid.NewGuid()) { Name = "  Acme  " };

            var context = harness.Context(Messages.Update, Stages.PreOperation, Account.EntityLogicalName);
            context.InputParameters["Target"] = target;

            harness.Execute<AtualizarNomePlugin>(context);

            Assert.Equal("Acme", target.Name);
        }

        [Fact]
        public void Nao_dispara_em_step_diferente()
        {
            var harness = new PluginHarness();
            var target = new Account(Guid.NewGuid()) { Name = "  Acme  " };

            // Stage errado (Post em vez de Pre): o PluginBase não casa o evento → handler não roda.
            var context = harness.Context(Messages.Update, Stages.PostOperation, Account.EntityLogicalName);
            context.InputParameters["Target"] = target;

            harness.Execute<AtualizarNomePlugin>(context);

            Assert.Equal("  Acme  ", target.Name); // intacto
        }
    }
}
