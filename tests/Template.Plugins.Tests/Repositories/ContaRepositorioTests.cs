using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Template.Plugins.Contas;
using Template.Plugins.Contatos;
namespace Template.Plugins.Tests
{
    public class ContaRepositorioTests
    {
        private static ContaRepositorio Semear()
        {
            var repo = new ContaRepositorio(new FakeOrganizationService());
            repo.Criar(new Conta { Nome = "Contoso", Receita = 2_000_000m, Categoria = CategoriaConta.ClientePreferencial, Estado = EstadoConta.Ativo, NumeroDeFuncionarios = 500, Servicos = new[] { Servico.Consultoria, Servico.Suporte } });
            repo.Criar(new Conta { Nome = "Fabrikam", Receita = 50_000m, Categoria = CategoriaConta.Padrao, Estado = EstadoConta.Ativo, NumeroDeFuncionarios = 10, Servicos = new[] { Servico.Treinamento } });
            repo.Criar(new Conta { Nome = "Contoso Brasil", Receita = 800_000m, Categoria = CategoriaConta.Padrao, Estado = EstadoConta.Inativo, NumeroDeFuncionarios = 80 });
            return repo;
        }

        [Fact]
        public void ObterPorNome_igualdade()
            => Assert.Equal("Fabrikam", Semear().ObterPorNome("Fabrikam").Nome);

        [Fact]
        public void BuscarPorNome_like_ordenado()
            => Assert.Equal(new[] { "Contoso", "Contoso Brasil" }, Semear().BuscarPorNome("contoso").Select(c => c.Nome).ToArray());

        [Fact]
        public void ListarPorCategoria_in()
        {
            var r = Semear().ListarPorCategoria(CategoriaConta.ClientePreferencial);
            Assert.Single(r);
            Assert.Equal("Contoso", r[0].Nome);
        }

        [Fact]
        public void TopPorReceita_money_ordem_desc()
            => Assert.Equal(new[] { "Contoso", "Contoso Brasil" }, Semear().TopPorReceita(100_000m, 5).Select(c => c.Nome).ToArray());

        [Fact]
        public void ListarAtivasPreferenciaisOuGrandes_composto()
        {
            var r = Semear().ListarAtivasPreferenciaisOuGrandes(100);
            Assert.Single(r);
            Assert.Equal("Contoso", r[0].Nome);
        }

        [Fact]
        public void ComServico_multiselect_containvalues()
        {
            var r = Semear().ComServico(Servico.Consultoria);
            Assert.Single(r);
            Assert.Equal("Contoso", r[0].Nome);
        }

        [Fact]
        public void NomesDosContatosPrincipais_link_entity()
        {
            var crm = new FakeOrganizationService();
            var repo = new ContaRepositorio(crm);
            var contatoId = crm.Create(new Contato(Guid.NewGuid()) { NomeCompleto = "Ada Lovelace" });
            var contaId = repo.Criar(new Conta
            {
                Nome = "Contoso",
                ContatoPrincipalId = new EntityReference(Contato.EntityLogicalName, contatoId)
            });

            var mapa = repo.NomesDosContatosPrincipais();

            Assert.Equal("Ada Lovelace", mapa[contaId]);
        }

        [Fact]
        public void AssociarContatos_relacionamento_NN()
        {
            var crm = new FakeOrganizationService();
            var repo = new ContaRepositorio(crm);
            var contaId = Guid.NewGuid();
            var c1 = Guid.NewGuid();
            var c2 = Guid.NewGuid();

            repo.AssociarContatos(contaId, "tpl_conta_contato", new[] { c1, c2 });

            var assoc = Assert.Single(crm.Associacoes);
            Assert.Equal("tpl_conta_contato", assoc.Relationship);
            Assert.Equal(contaId, assoc.EntityId);
            Assert.Equal(new[] { c1, c2 }, assoc.Related.ToArray());
        }
    }
}
