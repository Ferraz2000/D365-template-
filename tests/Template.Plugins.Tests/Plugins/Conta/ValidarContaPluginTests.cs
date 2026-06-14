using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Contas;
using Template.Plugins.Tests.Fakes;
using Xunit;

namespace Template.Plugins.Tests
{
    public class ValidarContaPluginTests
    {
        private static void Validar(Conta target)
        {
            var harness = new PluginHarness();
            var ctx = harness.Context(Messages.Create, Stages.PreValidation, Conta.EntityLogicalName);
            ctx.InputParameters["Target"] = target;
            harness.Execute<ValidarContaPlugin>(ctx);
        }

        [Fact]
        public void Aceita_conta_valida()
        {
            var ex = Record.Exception(() => Validar(new Conta(Guid.NewGuid()) { Nome = "Contoso", Receita = 10m }));
            Assert.Null(ex);
        }

        [Fact]
        public void Rejeita_nome_vazio()
            => Assert.Throws<InvalidPluginExecutionException>(() => Validar(new Conta(Guid.NewGuid()) { Nome = "   " }));

        [Fact]
        public void Rejeita_receita_negativa()
            => Assert.Throws<InvalidPluginExecutionException>(() => Validar(new Conta(Guid.NewGuid()) { Receita = -1m }));
    }
}
