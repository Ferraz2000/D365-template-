using System;
using Microsoft.Xrm.Sdk;
using Xunit;
using Account = Template.Plugins.Model.Account;
using AccountCategory = Template.Plugins.Model.AccountCategory;
using AccountState = Template.Plugins.Model.AccountState;

namespace Template.Plugins.Tests
{
    public class AccountModelTests
    {
        [Fact]
        public void Money_OptionSet_State_Int_DateTime_fazem_roundtrip()
        {
            var data = new DateTime(2026, 6, 14, 12, 0, 0, DateTimeKind.Utc);
            var account = new Account(Guid.NewGuid())
            {
                Revenue = 1500.50m,
                Category = AccountCategory.PreferredCustomer,
                State = AccountState.Active,
                NumberOfEmployees = 42,
                LastOnHoldTime = data
            };

            // Tipado de saída
            Assert.Equal(1500.50m, account.Revenue);
            Assert.Equal(AccountCategory.PreferredCustomer, account.Category);
            Assert.Equal(AccountState.Active, account.State);
            Assert.Equal(42, account.NumberOfEmployees);
            Assert.Equal(data, account.LastOnHoldTime);

            // Guardado nos tipos certos do SDK por baixo
            Assert.IsType<Money>(account["revenue"]);
            Assert.Equal(1500.50m, ((Money)account["revenue"]).Value);
            Assert.IsType<OptionSetValue>(account["accountcategorycode"]);
            Assert.Equal(1, ((OptionSetValue)account["accountcategorycode"]).Value);
        }

        [Fact]
        public void Propriedades_nulas_quando_ausentes()
        {
            var account = new Account();
            Assert.Null(account.Revenue);
            Assert.Null(account.Category);
            Assert.Null(account.NumberOfEmployees);
        }
    }
}
