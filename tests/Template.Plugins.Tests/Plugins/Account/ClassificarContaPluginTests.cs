using System;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using AccountCategory = Template.Plugins.Model.AccountCategory;
using ClassificarContaPlugin = Template.Plugins.Plugins.Account.ClassificarContaPlugin;

namespace Template.Plugins.Tests
{
    public class ClassificarContaPluginTests
    {
        [Theory]
        [InlineData(2_000_000, AccountCategory.PreferredCustomer)]
        [InlineData(500_000, AccountCategory.Standard)]
        public void Classifica_pela_receita(double receita, AccountCategory esperado)
        {
            var harness = new PluginHarness();
            var target = new Account(Guid.NewGuid()) { Revenue = (decimal)receita };

            var context = harness.Context(Messages.Update, Stages.PreOperation, Account.EntityLogicalName);
            context.InputParameters["Target"] = target;

            harness.Execute<ClassificarContaPlugin>(context);

            Assert.Equal(esperado, target.Category);
        }
    }
}
