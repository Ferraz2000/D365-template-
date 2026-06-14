using System.Linq;
using Template.Plugins.Repositories;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Conta = Template.Plugins.Model.Conta;
using CategoriaConta = Template.Plugins.Model.CategoriaConta;
using EstadoConta = Template.Plugins.Model.EstadoConta;
using Servico = Template.Plugins.Model.Servico;

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
    }
}
