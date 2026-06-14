using System.Linq;
using Template.Plugins.Repositories;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;
using AccountCategory = Template.Plugins.Model.AccountCategory;
using AccountState = Template.Plugins.Model.AccountState;

namespace Template.Plugins.Tests
{
    public class AccountRepositoryTests
    {
        private static AccountRepository Seed()
        {
            var repo = new AccountRepository(new FakeOrganizationService());
            repo.Create(new Account { Name = "Contoso", Revenue = 2_000_000m, Category = AccountCategory.PreferredCustomer, State = AccountState.Active, NumberOfEmployees = 500 });
            repo.Create(new Account { Name = "Fabrikam", Revenue = 50_000m, Category = AccountCategory.Standard, State = AccountState.Active, NumberOfEmployees = 10 });
            repo.Create(new Account { Name = "Contoso Brasil", Revenue = 800_000m, Category = AccountCategory.Standard, State = AccountState.Inactive, NumberOfEmployees = 80 });
            return repo;
        }

        [Fact]
        public void GetByName_igualdade()
        {
            var found = Seed().GetByName("Fabrikam");
            Assert.Equal("Fabrikam", found.Name);
        }

        [Fact]
        public void SearchByNameLike_trecho_e_ordenado()
        {
            var result = Seed().SearchByNameLike("contoso");
            Assert.Equal(new[] { "Contoso", "Contoso Brasil" }, result.Select(a => a.Name).ToArray());
        }

        [Fact]
        public void FindByCategory_optionset_In()
        {
            var result = Seed().FindByCategory(AccountCategory.PreferredCustomer);
            Assert.Single(result);
            Assert.Equal("Contoso", result[0].Name);
        }

        [Fact]
        public void TopByRevenue_money_filtro_ordem_desc()
        {
            var result = Seed().TopByRevenue(100_000m, 5);
            // só Contoso (2M) e Contoso Brasil (800k); Fabrikam (50k) fica de fora
            Assert.Equal(new[] { "Contoso", "Contoso Brasil" }, result.Select(a => a.Name).ToArray());
        }

        [Fact]
        public void FindActivePreferredOrBig_filtro_composto()
        {
            // Ativo E (preferencial OU >=100 funcionários): Contoso (preferencial). Fabrikam ativo mas standard/10.
            var result = Seed().FindActivePreferredOrBig(100);
            Assert.Single(result);
            Assert.Equal("Contoso", result[0].Name);
        }
    }
}
