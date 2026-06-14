using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// Responsabilidade única: normaliza o nome da conta antes de gravar.
    /// Registro: message=Update, stage=Pre-Operation, entity=account, filtro=name.
    /// </summary>
    public sealed class AtualizarNomePlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            if (string.IsNullOrWhiteSpace(account.Name)) return;

            // Pre-Operation: alterar a entidade tipada reflete no Target (AttributeCollection compartilhado).
            account.Name = account.Name.Trim();
            context.Trace("Nome da conta normalizado.");
        }
    }
}
