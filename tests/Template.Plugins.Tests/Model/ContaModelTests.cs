using System;
using Microsoft.Xrm.Sdk;
using Xunit;
using Conta = Template.Plugins.Model.Conta;
using CategoriaConta = Template.Plugins.Model.CategoriaConta;
using EstadoConta = Template.Plugins.Model.EstadoConta;
using Servico = Template.Plugins.Model.Servico;

namespace Template.Plugins.Tests
{
    public class ContaModelTests
    {
        [Fact]
        public void Tipos_fazem_roundtrip()
        {
            var conta = new Conta(Guid.NewGuid())
            {
                Receita = 1500.50m,
                Categoria = CategoriaConta.ClientePreferencial,
                Estado = EstadoConta.Ativo,
                NumeroDeFuncionarios = 42,
                Servicos = new[] { Servico.Consultoria, Servico.Suporte }
            };

            Assert.Equal(1500.50m, conta.Receita);
            Assert.Equal(CategoriaConta.ClientePreferencial, conta.Categoria);
            Assert.Equal(EstadoConta.Ativo, conta.Estado);
            Assert.Equal(42, conta.NumeroDeFuncionarios);
            Assert.Equal(new[] { Servico.Consultoria, Servico.Suporte }, conta.Servicos);

            // tipos do SDK por baixo
            Assert.IsType<Money>(conta["revenue"]);
            Assert.Equal(1500.50m, ((Money)conta["revenue"]).Value);
            Assert.IsType<OptionSetValueCollection>(conta["tpl_servicos"]);
            Assert.Equal(2, ((OptionSetValueCollection)conta["tpl_servicos"]).Count);
        }

        [Fact]
        public void Multiselect_vazio_e_nulos_quando_ausentes()
        {
            var conta = new Conta();
            Assert.Empty(conta.Servicos);
            Assert.Null(conta.Receita);
            Assert.Null(conta.Categoria);
        }
    }
}
