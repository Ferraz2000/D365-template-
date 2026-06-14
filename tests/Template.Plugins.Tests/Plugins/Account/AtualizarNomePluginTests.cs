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
        public void Apara_espacos_do_nome_no_pre_operation()
        {
            // Arrange — entidade tipada (early-bound)
            var harness = new PluginHarness();
            var target = new Account(Guid.NewGuid()) { Name = "  Acme  " };

            var context = harness.Context(Messages.Update, Stages.PreOperation);
            context.InputParameters["Target"] = target;

            // Act
            harness.Execute<AtualizarNomePlugin>(context);

            // Assert — ToEntity compartilha os atributos, então a alteração reflete no Target
            Assert.Equal("Acme", target.Name);
        }

        [Fact]
        public void Ignora_quando_nao_ha_nome_no_target()
        {
            var harness = new PluginHarness();
            var target = new Account(Guid.NewGuid());

            var context = harness.Context(Messages.Update, Stages.PreOperation);
            context.InputParameters["Target"] = target;

            var ex = Record.Exception(() => harness.Execute<AtualizarNomePlugin>(context));

            Assert.Null(ex);
            Assert.False(target.Contains(Account.Fields.Name));
        }
    }
}
