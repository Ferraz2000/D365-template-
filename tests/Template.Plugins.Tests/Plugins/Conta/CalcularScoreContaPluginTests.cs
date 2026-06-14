using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class CalcularScoreContaPluginTests
    {
        [Fact]
        public void Custom_message_calcula_score_e_devolve_no_output()
        {
            var harness = new PluginHarness();
            var contaId = harness.Service.Create(new Conta { Receita = 2_000_000m, NumeroDeFuncionarios = 150 });

            var context = harness.Context(CustomMessages.CalcularScoreConta, Stages.MainOperation);
            context.InputParameters["AccountId"] = new EntityReference(Conta.EntityLogicalName, contaId);

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
