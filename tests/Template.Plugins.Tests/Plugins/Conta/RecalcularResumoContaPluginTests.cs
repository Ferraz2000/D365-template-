using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Conta = Template.Plugins.Model.Conta;
using CategoriaConta = Template.Plugins.Model.CategoriaConta;
using RecalcularResumoContaPlugin = Template.Plugins.Plugins.Conta.RecalcularResumoContaPlugin;

namespace Template.Plugins.Tests
{
    public class RecalcularResumoContaPluginTests
    {
        [Fact]
        public void Depth_1_atualiza_o_resumo()
        {
            var harness = new PluginHarness();
            var id = harness.Service.Create(new Conta { Categoria = CategoriaConta.Padrao, Receita = 100m });

            var context = harness.Context(Messages.Update, Stages.PostOperation, Conta.EntityLogicalName);
            context.Depth = 1;
            context.PrimaryEntityId = id;
            context.InputParameters["Target"] = new Conta(id) { Categoria = CategoriaConta.Padrao, Receita = 100m };

            harness.Execute<RecalcularResumoContaPlugin>(context);

            var salvo = harness.Service.Retrieve(Conta.EntityLogicalName, id, new ColumnSet(true)).ToEntity<Conta>();
            Assert.False(string.IsNullOrEmpty(salvo.Resumo));
        }

        [Fact]
        public void Depth_2_nao_reentra_anti_loop()
        {
            var harness = new PluginHarness();
            var id = harness.Service.Create(new Conta());

            var context = harness.Context(Messages.Update, Stages.PostOperation, Conta.EntityLogicalName);
            context.Depth = 2; // reentrada
            context.PrimaryEntityId = id;
            context.InputParameters["Target"] = new Conta(id);

            harness.Execute<RecalcularResumoContaPlugin>(context);

            var salvo = harness.Service.Retrieve(Conta.EntityLogicalName, id, new ColumnSet(true)).ToEntity<Conta>();
            Assert.True(string.IsNullOrEmpty(salvo.Resumo));
        }
    }
}
