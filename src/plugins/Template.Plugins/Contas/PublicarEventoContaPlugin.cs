using System;
using Template.Plugins.Common;

namespace Template.Plugins.Contas
{
    /// <summary>
    /// **Integração desacoplada (recomendada)**: publica o contexto da operação numa fila do
    /// Azure Service Bus via ServiceEndpoint, em vez de chamar o sistema externo direto.
    /// Registrar: Create/Update / Post-Operation / account / **async**.
    /// </summary>
    public sealed class PublicarEventoContaPlugin : PluginBase
    {
        // Em produção, o id do ServiceEndpoint vem de uma Environment Variable / config do step.
        private static readonly Guid ServiceEndpointId = new Guid("00000000-0000-0000-0000-000000000001");

        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Conta>(out _)) return;

            context.PostarNaFila(ServiceEndpointId);
            context.Trace("Evento da conta publicado na fila.");
        }
    }
}
