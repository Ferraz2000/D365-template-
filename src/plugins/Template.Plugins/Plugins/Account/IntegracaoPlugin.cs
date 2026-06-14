using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 plugin = 1 step: após criar a conta, enfileirar integração.
    /// Registrar em: message=Create, stage=Post-Operation, entity=account, modo=async (recomendado).
    /// </summary>
    public sealed class IntegracaoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;

            var accountId = context.PluginContext.PrimaryEntityId;
            context.Trace($"Enfileirar integração da conta {accountId} ({account.Name}).");

            // TODO: integração real (fila/serviço).
        }
    }
}
