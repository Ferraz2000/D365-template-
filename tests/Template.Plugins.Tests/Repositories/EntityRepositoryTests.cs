using System;
using Template.Plugins.Repositories;
using Template.Plugins.Tests.Fakes;
using Xunit;
using Account = Template.Plugins.Model.Account;

namespace Template.Plugins.Tests
{
    public class EntityRepositoryTests
    {
        [Fact]
        public void Create_e_Retrieve_tipado_persistem_a_entidade()
        {
            var harness = new PluginHarness();
            var repo = new EntityRepository(harness.Service);

            var id = repo.Create(new Account { Name = "Contoso" });
            var account = repo.Retrieve<Account>(id, Account.Fields.Name);

            Assert.Equal("Contoso", account.Name);
        }

        [Fact]
        public void Construtor_rejeita_service_nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityRepository(null));
        }
    }
}
