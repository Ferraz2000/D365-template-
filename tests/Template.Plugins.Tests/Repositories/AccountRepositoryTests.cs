using Template.Plugins.Repositories;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;

namespace Template.Plugins.Tests
{
    public class AccountRepositoryTests
    {
        [Fact]
        public void GetByName_filtra_pela_query_do_repositorio()
        {
            var repo = new AccountRepository(new FakeOrganizationService());
            repo.Create(new Account { Name = "Contoso" });
            repo.Create(new Account { Name = "Fabrikam" });

            var found = repo.GetByName("Fabrikam");

            Assert.NotNull(found);
            Assert.Equal("Fabrikam", found.Name);
        }

        [Fact]
        public void GetById_retorna_tipado()
        {
            var repo = new AccountRepository(new FakeOrganizationService());
            var id = repo.Create(new Account { Name = "Contoso" });

            var found = repo.GetById(id, Account.Fields.Name);

            Assert.Equal("Contoso", found.Name);
        }
    }
}
