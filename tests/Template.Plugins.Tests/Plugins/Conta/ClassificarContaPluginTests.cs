using System;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Conta = Template.Plugins.Model.Conta;
using CategoriaConta = Template.Plugins.Model.CategoriaConta;
using ClassificarContaPlugin = Template.Plugins.Plugins.Conta.ClassificarContaPlugin;

namespace Template.Plugins.Tests
{
    public class ClassificarContaPluginTests
    {
        [Theory]
        [InlineData(2_000_000, CategoriaConta.ClientePreferencial)]
        [InlineData(500_000, CategoriaConta.Padrao)]
        public void Classifica_pela_receita(double receita, CategoriaConta esperado)
        {
            var harness = new PluginHarness();
            var target = new Conta(Guid.NewGuid()) { Receita = (decimal)receita };

            var context = harness.Context(Messages.Update, Stages.PreOperation, Conta.EntityLogicalName);
            context.InputParameters["Target"] = target;

            harness.Execute<ClassificarContaPlugin>(context);

            Assert.Equal(esperado, target.Categoria);
        }
    }
}
