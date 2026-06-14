using System.Net.Http;
using Template.Plugins.Common;
using Template.Plugins.Integracao;

namespace Template.Plugins.Plugins.Conta
{
    /// <summary>
    /// **Integração HTTP** (REST) após criar a conta. Registrar: Create / Post-Operation / account / **async**.
    /// Envia um payload ao sistema externo. Em projeto real a URL vem de Environment Variable.
    /// </summary>
    public sealed class IntegracaoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Conta>(out var conta)) return;

            var contaId = context.PluginContext.PrimaryEntityId;
            var json = $"{{\"id\":\"{contaId}\",\"nome\":{Json(conta.Nome)}}}"; // real: usar um serializador

            new ClienteRest(new HttpClient()).PostJson("https://exemplo.com/api/contas", json);
            context.Trace($"Conta {contaId} enviada ao sistema externo.");
        }

        private static string Json(string valor) => valor == null ? "null" : "\"" + valor.Replace("\"", "\\\"") + "\"";
    }
}
