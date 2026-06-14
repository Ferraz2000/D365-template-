using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 plugin = 1 step: normalizar o nome da conta.
    /// Registrar em: message=Update, stage=Pre-Operation, entity=account, filtro=name.
    /// Regra simples → fica aqui mesmo.
    /// </summary>
    public sealed class AtualizarNomePlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            if (string.IsNullOrWhiteSpace(account.Name)) return;

            account.Name = account.Name.Trim();
            context.Trace("Nome da conta normalizado.");
        }
    }
}
