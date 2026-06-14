using System;
using Template.Plugins.Contas;
using Xunit;

namespace Template.Plugins.Tests
{
    public class ContaPayloadTests
    {
        [Fact]
        public void Json_inclui_id_nome_e_receita()
        {
            var id = Guid.NewGuid();
            var json = ContaPayload.Json(new Conta { Nome = "Contoso", Receita = 1234.5m }, id);

            Assert.Contains($"\"id\":\"{id}\"", json);
            Assert.Contains("\"nome\":\"Contoso\"", json);
            Assert.Contains("\"receita\":1234.5", json); // invariant culture
        }

        [Fact]
        public void Json_escapa_aspas_e_trata_nulos()
        {
            var json = ContaPayload.Json(new Conta { Nome = "A\"B" }, Guid.Empty);
            Assert.Contains("A\\\"B", json);
            Assert.Contains("\"receita\":null", json);
        }
    }
}
