using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class ContaServicoTests
    {
        private static ContaServico NovoServico(FakeOrganizationService crm)
            => new ContaServico(new ContaRepositorio(crm), new ContatoRepositorio(crm));

        [Fact]
        public void RejeitarSeNomeDuplicado_barra_existente()
        {
            var crm = new FakeOrganizationService();
            new ContaRepositorio(crm).Criar(new Conta { Nome = "Contoso" });

            Assert.Throws<InvalidPluginExecutionException>(() =>
                NovoServico(crm).RejeitarSeNomeDuplicado(new Conta(Guid.NewGuid()) { Nome = "Contoso" }));
        }

        [Fact]
        public void RejeitarSeNomeDuplicado_permite_unico()
        {
            var crm = new FakeOrganizationService();
            var ex = Record.Exception(() => NovoServico(crm).RejeitarSeNomeDuplicado(new Conta(Guid.NewGuid()) { Nome = "Fabrikam" }));
            Assert.Null(ex);
        }

        [Fact]
        public void PropagarContatoPrincipal_vincula()
        {
            var crm = new FakeOrganizationService();
            var contatoId = crm.Create(new Contato(Guid.NewGuid()));

            var conta = new Conta(Guid.NewGuid())
            {
                ContatoPrincipalId = new EntityReference(Contato.EntityLogicalName, contatoId)
            };

            NovoServico(crm).PropagarContatoPrincipal(conta);

            var contato = crm.Retrieve(Contato.EntityLogicalName, contatoId, new ColumnSet(true)).ToEntity<Contato>();
            Assert.Equal(conta.Id, contato.ClientePaiId.Id);
        }
    }
}
