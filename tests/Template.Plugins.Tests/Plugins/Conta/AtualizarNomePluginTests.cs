using System;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class AtualizarNomePluginTests
    {
        [Fact]
        public void Normaliza_o_nome_da_conta()
        {
            var harness = new PluginHarness();
            var target = new Conta(Guid.NewGuid()) { Nome = "  Acme  " };

            var context = harness.Context(Messages.Update, Stages.PreOperation, Conta.EntityLogicalName);
            context.InputParameters["Target"] = target;

            harness.Execute<AtualizarNomePlugin>(context);

            Assert.Equal("Acme", target.Nome);
        }
    }
}
