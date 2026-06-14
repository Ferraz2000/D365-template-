using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using CalcularScoreContaPlugin = Template.Plugins.Plugins.Account.CalcularScoreContaPlugin;

namespace Template.Plugins.Tests
{
    public class CalcularScoreContaPluginTests
    {
        [Fact]
        public void Custom_message_calcula_score_e_devolve_no_output()
        {
            var harness = new PluginHarness();
            var accountId = harness.Service.Create(new Account { Revenue = 2_000_000m, NumberOfEmployees = 150 });

            var context = harness.Context(CustomMessages.CalcularScoreConta, Stages.MainOperation);
            context.InputParameters["AccountId"] = new EntityReference(Account.EntityLogicalName, accountId);

            harness.Execute<CalcularScoreContaPlugin>(context);

            Assert.Equal(80, context.OutputParameters["Score"]); // 50 (receita) + 30 (funcionários)
        }

        [Fact]
        public void Exige_o_parametro_AccountId()
        {
            var harness = new PluginHarness();
            var context = harness.Context(CustomMessages.CalcularScoreConta, Stages.MainOperation);

            Assert.Throws<InvalidPluginExecutionException>(() => harness.Execute<CalcularScoreContaPlugin>(context));
        }
    }
}
