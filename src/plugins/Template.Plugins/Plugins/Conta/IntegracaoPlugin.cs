using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Conta
{
    /// <summary>Post-Operation (async): enfileira integração após criar. Registrar: Create / Post-Operation / account.</summary>
    public sealed class IntegracaoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Conta>(out var conta)) return;

            var contaId = context.PluginContext.PrimaryEntityId;
            context.Trace($"Enfileirar integração da conta {contaId} ({conta.Nome}).");
            // TODO: integração real (fila/serviço).
        }
    }
}
