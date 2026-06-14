using System;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Repositories;
using Xunit;

namespace Template.Plugins.Tests.Repositories
{
    public class EntityRepositoryTests
    {
        [Fact]
        public void Create_e_Retrieve_persistem_a_entidade()
        {
            var ctx = new XrmFakedContext();
            var repo = new EntityRepository(ctx.GetOrganizationService());

            var id = repo.Create(new Entity("account") { ["name"] = "Contoso" });
            var account = repo.Retrieve("account", id, "name");

            Assert.Equal("Contoso", account.GetAttributeValue<string>("name"));
        }

        [Fact]
        public void Construtor_rejeita_service_nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityRepository(null));
        }
    }
}
