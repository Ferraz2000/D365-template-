using System.Net.Http;
using Template.Plugins.Common;
using Template.Plugins.Integracao;

namespace Template.Plugins.Contas
{
    /// <summary>
    /// **Integração HTTP** (REST) após criar a conta. Registrar: Create / Post-Operation / account / **async**.
    /// Envia um payload ao sistema externo. Em projeto real a URL vem de Environment Variable.
    /// </summary>
    public sealed class IntegracaoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Conta>(out var conta)) return;

            var contaId = context.PluginContext.PrimaryEntityId;
            var json = ContaPayload.Json(conta, contaId);

            new ClienteRest(new HttpClient()).PostJson("https://exemplo.com/api/contas", json);
            context.Trace($"Conta {contaId} enviada ao sistema externo.");
        }
    }
}
