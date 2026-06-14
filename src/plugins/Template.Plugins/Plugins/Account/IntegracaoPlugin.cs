using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// Responsabilidade única: dispara a integração externa após criar a conta.
    /// Totalmente separado dos outros plugins da Account (nada acoplado).
    /// Registro: message=Create, stage=Post-Operation, entity=account, modo=async (recomendado).
    /// </summary>
    public sealed class IntegracaoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget(out var target)) return;

            var accountId = context.PluginContext.PrimaryEntityId; // id já existe no Post
            context.Trace($"Enfileirar integração da conta {accountId}.");

            // TODO: chamada à integração (fila/serviço). Mantenha fino: delegue a um serviço.
        }
    }
}
